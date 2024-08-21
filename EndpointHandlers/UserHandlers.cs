using AutoMapper;
using Concessionaria.Context;
using Concessionaria.Entities;
using Concessionaria.Models;
using Concessionaria.Models.Users;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Concessionaria.Service;
using Microsoft.OpenApi.Any;
using Microsoft.AspNetCore.Authorization;

namespace Concessionaria.EndpointHandlers;

public static class UserHandlers
{
    public static async Task<Results<NotFound, Ok<IEnumerable<UserDTO>>>>
                        GetUsers(OrganizadorContext DB,
                                 IMapper mapper,
                                 [FromQuery(Name = "Name")]
                                 string? name
                                 ) 
    {
        var users = mapper.Map<IEnumerable<UserDTO>>(await DB.Users.Where(x => name == null || name.ToUpper().Contains(x.Name.ToUpper())).ToListAsync());

        if(users.Count() == 0 || users.Count() == 0)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(users);
    }

    [Authorize]
    public static async Task<Results<NotFound, Ok<UserDTO>>>
                      GetUsersById(OrganizadorContext DB,
                                   IMapper mapper,
                                   Guid? Id)
    {
        if (Id == null)
            return TypedResults.NotFound();

        var user = await DB.Users.FirstOrDefaultAsync(x => x.IdUser == Id);
        if (user == null)
            return TypedResults.NotFound();

        var userDto = mapper.Map<UserDTO>(user);
        return TypedResults.Ok(userDto);
    }

    public static async Task<Results<BadRequest<string>, CreatedAtRoute<UserDTO>>>
                            PostUser(OrganizadorContext DB,
                                     HashService hashService,
                                     IMapper mapper,
                                     [FromBody] 
                                     UserForCreateDTO UserCreate)
    {
        if (UserCreate == null || await DB.Users.AnyAsync(x => x.Email == UserCreate.Email))
            return TypedResults.BadRequest("Email já em uso.");
        
        var User = mapper.Map<User>(UserCreate);
        hashService.RegisterUser(User, UserCreate.Password);

        DB.Users.Add(User);
        await DB.SaveChangesAsync();

        var UserReturn = mapper.Map<UserDTO>(User);

        return TypedResults.CreatedAtRoute(UserReturn,
                                           "GetCarroId",
                                           new
                                           {
                                               IdCar = UserReturn.IdUser
                                           });
    }

    public static async Task<Results<NotFound,Ok>>
                        DeleteUser(OrganizadorContext DB,
                                   Guid? Id)
    {
        if (Id == null)
            return TypedResults.NotFound();

        var user = await DB.Users.FirstOrDefaultAsync(x => Id == x.IdUser);  

        DB.Users.Remove(user);
        DB.SaveChanges();

        return TypedResults.Ok();
    }

    public static async Task<Results<NotFound<string>, Ok>>
                                    PutUser(OrganizadorContext DB,
                                            IMapper mapper,
                                            [FromBody]
                                            UserForAlterationDTO UserForAlteration,
                                            Guid Id)
    {
       var user = await DB.Users.FirstOrDefaultAsync(x => x.IdUser == Id);

        if(user == null)
            return TypedResults.NotFound("Não foi possivel alterar");

        mapper.Map(UserForAlteration, user);
        DB.SaveChanges();

        return TypedResults.Ok();
    }

    public static async Task<Results<BadRequest, Ok<object>>> LoginRequest(
    OrganizadorContext DB,
    IMapper mapper,
    HashService HashService,
    GenerateToken generateToken,
    [FromBody] LoginDTO loginRequest)
    {
        // Busca o usuário pelo email
        var login = await DB.Users.FirstOrDefaultAsync(x => loginRequest.Email == x.Email);

        // Se o usuário não for encontrado ou a senha estiver incorreta, retorna BadRequest
        if (login == null || !HashService.ValidateUser(login, loginRequest.Password))
        {
            return TypedResults.BadRequest();
        }

        // Gera o token JWT
        var token = generateToken.GenerateTokenLogin(loginRequest.Email);

        // Retorna o token em uma resposta OK
        return TypedResults.Ok((object)new { token });
    }

}
