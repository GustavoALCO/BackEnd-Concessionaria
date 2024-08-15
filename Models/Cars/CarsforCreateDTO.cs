using Concessionaria.Entities;
using Concessionaria.Service;


namespace Concessionaria.Models.Cars;

public class CarsforCreateDTO 
{
    public Guid IdCar { get; set; }

    public Guid IdUser { get; set; }

    public string CarBrand { get; set; }

    public string Model { get; set; }

    public string Color { get; set; }

    public int Age { get; set; }

    public string CarPlate { get; set; }

    public List<string> Url { get; set; } = new List<string>();

    public DateTimeOffset DateUpload { get; set; }

    
    public CarsforCreateDTO()
    {
        IdCar = Guid.NewGuid();
        DateUpload = DateTimeOffset.Now;
    }
}
