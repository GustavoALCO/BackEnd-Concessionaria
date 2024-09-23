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
                                     UserCreateDTO UserCreate)
    {
        if (UserCreate == null || await DB.Users.AnyAsync(x => x.Login == UserCreate.Login))
            return TypedResults.BadRequest("login já em uso.");
        

        var User = mapper.Map<User>(UserCreate);

        User.Auditable.CreateUserId = UserCreate.CreateUserId;  
        User.Auditable.DateUpload = DateTimeOffset.Now;
        hashService.RegisterUser(User, UserCreate.Password);


        DB.Users.Add(User);
        Console.WriteLine(User.Auditable.DateUpload);
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

        var User = await DB.Users.FirstOrDefaultAsync(x => Id == x.IdUser);  

        DB.Users.Remove(User);
        DB.SaveChanges();

        return TypedResults.Ok();
    }

    public static async Task<Results<NotFound<string>, Ok>>
                                    PutUser(OrganizadorContext DB,
                                            IMapper mapper,
                                            HashService hashService,
                                            [FromBody]
                                            UserAlterationDTO UserAlteration,
                                            Guid Id)
    {
       var User = await DB.Users.FirstOrDefaultAsync(x => x.IdUser == Id);

        if(User == null)
            return TypedResults.NotFound("Não foi possivel alterar");

        User.Auditable.AlterationUserId = UserAlteration.AlterationUserId;
        User.Auditable.DateUpdate = DateTimeOffset.Now;
        hashService.RegisterUser(User, UserAlteration.Password);

        mapper.Map(UserAlteration, User);
        DB.SaveChanges();

        return TypedResults.Ok();
    }

    public static async Task<Results<BadRequest, Ok<object>>> LoginRequest(
    OrganizadorContext DB,
    IMapper mapper,
    HashService HashService,
    GenerateToken generateToken,
    [FromBody] UserLoginDTO loginRequest)
    {
        // Busca o usuário pelo email
        var login = await DB.Users.FirstOrDefaultAsync(x => loginRequest.Login == x.Login);

        // Se o usuário não for encontrado ou a senha estiver incorreta, retorna BadRequest
        if (login == null || !HashService.ValidateUser(login, loginRequest.Password))
        {
            return TypedResults.BadRequest();
        }

        // Gera o token JWT
        var token = generateToken.GenerateTokenLogin(loginRequest.Login);

        // Retorna o token em uma resposta OK
        return TypedResults.Ok((object)new { token });
    }

}
