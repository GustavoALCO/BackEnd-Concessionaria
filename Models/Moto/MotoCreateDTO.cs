using Concessionaria.Entities;
using Concessionaria.Service;


namespace Concessionaria.Models.Cars;

public class MotoCreateDTO
{
    public Guid IdMoto { get; set; }
    public string MotoBrand { get; set; }
    public string Model { get; set; }
    public string Fuel { get; set; }
    public string Color { get; set; }
    public string Plate { get; set; }
    public int Age { get; set; }
    public int Km { get; set; }
    public int Price { get; set; }
    public string[] Url { get; set; } 

    //Classe Auditable 
    public DateTimeOffset DateUpload { get; set; } 
    public Guid CreateUserId { get; set; } 

    public int IdStore { get; set; }
}
