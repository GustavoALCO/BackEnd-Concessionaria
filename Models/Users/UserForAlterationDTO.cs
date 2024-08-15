using Concessionaria.Entities;
using System.ComponentModel.DataAnnotations;

namespace Concessionaria.Models.Users;

public class UserForAlterationDTO
{

    public string? Name { get; set; }

    public string? LastName { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? Cep { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }
}
