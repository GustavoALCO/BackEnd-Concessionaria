using AutoMapper;
using Concessionaria.Context;
using Concessionaria.Entities;
using Concessionaria.Models.Users;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        if(users.Count() == null || users.Count() == 0)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(users);
    }

    public static async Task<Results<NotFound, Ok<IEnumerable<UserDTO>>>>
                         GetUsersById(OrganizadorContext DB,
                                      IMapper mapper,
                                      Guid? id
                                        )
    {
        var users = mapper.Map<IEnumerable<UserDTO>>(await DB.Users.FirstOrDefaultAsync(x => x.IdUser == id));

        if (users.Count() == null || users.Count() == 0)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(users);
    }

    public static async Task<Results<BadRequest, CreatedAtRoute<UserDTO>>>
                            PostUser(OrganizadorContext DB,
                                     IMapper mapper,
                                     [FromBody] 
                                     UserForCreateDTO UserCreate)
    {
        if (UserCreate == null)
            return TypedResults.BadRequest();

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
                                   Guid? id)
    {
        var user = await DB.Users.FirstOrDefaultAsync(x => id == x.IdUser); 

        if (id == null) 
            return TypedResults.NotFound();

        DB.Users.Remove(user);
        DB.SaveChanges();

        return TypedResults.Ok();
    }

    public static async Task<Results<NotFound, Ok>>
                                    PutUser(OrganizadorContext DB,
                                            IMapper mapper,
                                            [FromBody]
                                            UserDTO UserAlteration,
                                            Guid Id)
    {
       var user = await DB.Users.FirstOrDefaultAsync(x => x.IdUser == Id);

        if(user == null)
            return TypedResults.NotFound();

        mapper.Map(UserAlteration, user);
        DB.SaveChanges();

        return TypedResults.Ok();
    }

    public static async Task<Results<BadRequest,Ok>>
                                    LoginRequest(OrganizadorContext DB,
                                              IMapper mapper,
                                              [FromBody]
                                              UserLoginDTO UserLogin,
                                              string Email,
                                              string Password)
    {
        var login = await DB.Users.FirstOrDefaultAsync(x => x.Email == Email && x.Password == Password);

        if(login == null) 
            TypedResults.NotFound();

        return TypedResults.Ok();
    }//ESPERANDO IMPLEMENTAÇÃO DOS TOKENS JWT
}
