using Concessionaria.Entities;
using System.ComponentModel.DataAnnotations;

namespace Concessionaria.Models.Users;

public class UserDTO
{
    //Classe de User
    public Guid IdUser { get; set; }

    public int IdStore { get; set; }

    public string Name { get; set; }

    public string LastName { get; set; }

    public string Role { get; set; }

    public string Login { get; set; }

    public string Password { get; set; }

    //Classe Auditable
    public DateTimeOffset DateUpload { get; set; } // Data quando a moto é criada
    public Guid CreateUserId { get; set; } // ID do usuário que criou o anuncio
    public DateTimeOffset? DateUpdate { get; set; } // Data quando a moto é alterada pela ultima vez
    public Guid? AlterationUserId { get; set; } // ID do usuário que fez a última alteração
}
