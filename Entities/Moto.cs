using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

    public string Fuel { get; set; }
    //Variavel para atribuir qual será o combustivel 

    public string Color { get; set; }
    //Variavel para atribuir a cor do carro

    public string Plate { get; set; }
    //Variavel para atribuir a placa do carro

    public int Age { get; set; }
    //Variavel para atribuir o ano do carro

    public int Km { get; set; }

    public int Price { get; set; }

    public string[] Url { get; set; }
    //Lista para Atribuir links das Imagens que fizeram upload na azure

    [ForeignKey("Store")]
    public int IdStore { get; set; }

    public Auditable Auditable { get; set; }


    public Moto()
    {
        
    }
}
