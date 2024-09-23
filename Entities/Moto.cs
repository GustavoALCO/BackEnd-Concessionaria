using System.ComponentModel.DataAnnotations;

namespace Concessionaria.Entities;

public class Moto 
{
    [Key]
    public Guid IdMoto { get; set; }
    //Id Gerado com GUID para ter pouca chance de se repetir

    public string MotoBrand { get; set; }
    //Variavel para atribuir o nome da fabricante do carro

    public string Model { get; set; }
    //Variavel para atribuir o nome do carro

    public string Description { get; set; }
    //Variavel para atribuir uma Descrição da moto anunciada

    public string Fuel { get; set; }
    //Variavel para atribuir qual será o combustivel 

    public string Color { get; set; }
    //Variavel para atribuir a cor do carro

    public int Age { get; set; }
    //Variavel para atribuir o ano do carro

    public List<string> Url { get; set; }
    //Lista para Atribuir links das Imagens que fizeram upload na azure

    public Auditable Auditable { get; set; }

    public Store Store { get; set; }

    public Moto()
    {
        
    }
}
