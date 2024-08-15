

namespace Concessionaria.Models.Cars;

public class CarsForAlterationDTO
{
    public string? CarBrand { get; set; }
    //Variavel para atribuir o nome da fabricante do carro

    public string? Model { get; set; }
    //Variavel para atribuir o nome do carro

    public string? Color { get; set; }
    //Variavel para atribuir a cor do carro

    public int? Age { get; set; }
    //Variavel para atribuir o ano do carro

    public string? CarPlate { get; set; }
    //Variavel para atribuir a placa do carro

    public DateTimeOffset DateUpdate { get; set; }

    public CarsForAlterationDTO()
    {
        DateUpdate = DateTimeOffset.Now;
    }
}
