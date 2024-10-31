using Concessionaria.Entities;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Concessionaria.Models.Users;

public class UserCreateDTO
{
    public Guid UserId { get; set; }

    public int IdStore { get; set; }

    public string Name { get; set; }

    public string LastName { get; set; }

    public string Role { get; set; }
    
    public string Login { get; set; }
    
    public string Password { get; set; }

    //Classe Auditable
    public Guid CreateUserId { get; set; }

    public DateTimeOffset DateUpload { get; set; } // Data quando a moto é criada
    

    public UserCreateDTO()
    {
        UserId = Guid.NewGuid();    
        DateUpload = DateTimeOffset.Now;
    }
}
