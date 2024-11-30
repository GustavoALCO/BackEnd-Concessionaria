namespace Concessionaria.Models.Users;

public class AlterPasswordDTO
{
    public string Password { get; set; }
    public Guid AlterationUserId { get; set; }
}
