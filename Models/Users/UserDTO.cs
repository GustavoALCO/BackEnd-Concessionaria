using Concessionaria.Entities;
using System.ComponentModel.DataAnnotations;

namespace Concessionaria.Models.Users;

public class UserDTO
{
    public Guid IdUser { get; set; }

    public string Name { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

}
