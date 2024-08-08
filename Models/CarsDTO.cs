using Concessionaria.Entities;
using System.ComponentModel.DataAnnotations;

namespace Concessionaria.Models;

public class CarsDTO
{
    public Guid IdCar { get; set; }
    //Id Gerado com GUID para ter pouca chance de se repetir

    public required string CarBrand { get; set; }
    //Variavel para atribuir o nome da fabricante do carro

    public required string Model { get; set; }
    //Variavel para atribuir o nome do carro

    public required string Color { get; set; }
    //Variavel para atribuir a cor do carro

    public required int Age { get; set; }
    //Variavel para atribuir o ano do carro

    public string CarPlate { get; set; }
    //Variavel para atribuir a placa do carro

    public ImageDTO Image { get; set; }
}
