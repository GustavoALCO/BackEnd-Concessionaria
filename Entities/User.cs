using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace Concessionaria.Entities;
public class User
{
    [Key]
    public Guid IdUser { get; set; }

    [Required]
    public string Name { get; set; }

    public string LastName { get; set; }

    public string PhoneNumber { get; set; }

    public string Address { get; set; }

    public string City { get; set; }

    public string Cep { get; set; }

    [Required, MaxLength(100)]
    public string Email { get; set; }

    public string Password { get; set; }

    public List<Cars> Carros { get; set; } = [];

    public User()
    {
    }
    public User(string name, string lastname, string email, string password)
    {
        IdUser = Guid.NewGuid();
        Name = name;
        LastName = lastname;
        Email = email;
        Password = password;
        Carros = new List<Cars>();
    }
}

