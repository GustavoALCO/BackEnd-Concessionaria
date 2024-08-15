using Concessionaria.Entities;
using System.Diagnostics.CodeAnalysis;

namespace Concessionaria.Models.Users;

public class UserForCreateDTO
{
    public Guid IdUser { get; set; }

    public required string Name { get; set; }

    public required string LastName { get; set; }

    public string? PhoneNumber { get; set; }

    public required string Address { get; set; }

    public required string City { get; set; }

    public string? Cep { get; set; }

    public required string Email { get; set; }

    public required string Password { get; set; }

    public UserForCreateDTO()
    {
        IdUser = Guid.NewGuid();
    }
}
