using AutoMapper;
using Concessionaria.Context;
using Concessionaria.Entities;
using Concessionaria.Models;
using Concessionaria.Models.Users;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Concessionaria.Service;

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
                                     IMapper mapper,
                                     [FromBody] 
                                     UserForCreateDTO UserCreate)
    {
        if (UserCreate == null || await DB.Users.AnyAsync(x => x.Email == UserCreate.Email))
            return TypedResults.BadRequest("Email já em uso.");

        var User = mapper.Map<User>(UserCreate);

        DB.Users.Add(User);
        DB.SaveChanges();

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

    public static async Task<Results<BadRequest,Ok>>
                                    LoginRequest(OrganizadorContext DB,
                                              IMapper mapper,
                                              [FromBody]
                                              LoginDTO loginRequest)
    {
        var login = await DB.Users.FirstOrDefaultAsync(x => loginRequest.Email == x.Email);

        if(login == null || loginRequest.Password == login.Password) 
            TypedResults.NotFound();

        return TypedResults.Ok();
    }//ESPERANDO IMPLEMENTAÇÃO DOS TOKENS JWT
}
