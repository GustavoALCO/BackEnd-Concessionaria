using Concessionaria.Entities;

namespace Concessionaria.Models.Users;

public class UserForCreateDTO
{
    public Guid IdUser { get; set; }

    public string Name { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

}
