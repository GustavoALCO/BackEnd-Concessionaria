using AutoMapper;
using Concessionaria.Context;
using Concessionaria.Entities;
using Concessionaria.Models.Users;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Concessionaria.Service;
using Microsoft.AspNetCore.Authorization;
using FluentValidation;

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
                                   Guid? Id
                                   )
    {
        if (Id == null)
            return TypedResults.NotFound();

        var user = await DB.Users.FindAsync(Id);
        if (user == null)
            return TypedResults.NotFound();

        var userDto = mapper.Map<UserDTO>(user);
        return TypedResults.Ok(userDto);
    }

    public static async Task<Results<BadRequest<string>, CreatedAtRoute<UserDTO>>>
                            PostUser(OrganizadorContext DB,
                                    IValidator<UserCreateDTO> validator,
                                     HashService hashService,
                                     IMapper mapper,
                                     [FromBody] 
                                     UserCreateDTO UserCreate)
    {
        if (UserCreate == null || await DB.Users.AnyAsync(x => x.Login == UserCreate.Login))
            return TypedResults.BadRequest("login já em uso.");
        
        var user = validator.Validate(UserCreate);
        if (!user.IsValid)
            return TypedResults.BadRequest(user.Errors.ToString());

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
    [Authorize]
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

    public static async Task<Results<BadRequest<string>, Ok>>
                                    PutUser(OrganizadorContext DB,
                                            IMapper mapper,
                                            IValidator<UserAlterationDTO> validator,
                                            HashService hashService,
                                            [FromBody]
                                            UserAlterationDTO UserAlteration,
                                            Guid Id)
    {
       var User = await DB.Users.FirstOrDefaultAsync(x => x.IdUser == Id);

        if(User == null)
            return TypedResults.BadRequest("Não foi possivel Achar o User");

        var user = validator.Validate(UserAlteration);
        if(!user.IsValid)
                return TypedResults.BadRequest(user.Errors.ToString());

        User.Auditable.AlterationUserId = UserAlteration.AlterationUserId;
        User.Auditable.DateUpdate = DateTimeOffset.Now;

        if(UserAlteration.Password != null)
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

        
        var token = generateToken.GerarTokenLogin(loginRequest.Login);

        // Retorna o token em uma resposta OK
        return TypedResults.Ok((object)new { token });
    }

}
