using System.ComponentModel.DataAnnotations;

namespace Concessionaria.Entities;

public class Store
{
    [Key]
    public int IdStore { get; set; }

    public string Adress { get; set; }

    public string CEP { get; set; }

    public bool IsFull { get; set; }

    public Store()
    {
        
    }
}
