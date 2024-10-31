using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Concessionaria.Entities;

public class User
{
    [Key]
    public Guid IdUser { get; set; }

    [ForeignKey("Store")]
    public int IdStore { get; set; }

    public string Name { get; set; }

    public string LastName { get; set; }
    
    public string Role {  get; set; }
    
    public string Login { get; set; }
    
    public string Password { get; set; }

    public Auditable Auditable { get; set; } = new Auditable();


}
