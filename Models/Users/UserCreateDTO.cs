using Concessionaria.Entities;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Concessionaria.Models.Users;

public class UserCreateDTO
{
    public Guid UserId { get; set; }
    [Required(ErrorMessage = "Nome é Necessario")]
    public string Name { get; set; }

    public string LastName { get; set; }

    public string Role { get; set; }
    [Required(ErrorMessage = "É precisso colocar uma forma de login")]
    public string Login { get; set; }
    [Required(ErrorMessage = "É precisso colocar uma senha")]
    public string Password { get; set; }

    //Classe Auditable
    public Guid? CreateUserId { get; set; }

    public DateTimeOffset DateUpload { get; set; } // Data quando a moto é criada
    

    public UserCreateDTO()
    {
        UserId = Guid.NewGuid();    
        DateUpload = DateTimeOffset.Now;
    }
}
