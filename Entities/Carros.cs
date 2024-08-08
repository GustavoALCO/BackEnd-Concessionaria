using System.ComponentModel.DataAnnotations;

namespace Concessionaria.Entities;

public class Carros
{
    [Key]
    [Required]
    public Guid IdCar { get; set; }
    //Id Gerado com GUID para ter pouca chance de se repetir
    public Guid IdUser { get; set; }
    // Chave estrangeira para Usuario
    public User User { get; set; }
    // Propriedade de navegação para Usuario
    
    [Required]
    [MaxLength(20)]
    public string CarBrand { get; set; }
    //Variavel para atribuir o nome da fabricante do carro
    
    [Required]
    [MaxLength(40)]
    public string Model { get; set; }
    //Variavel para atribuir o nome do carro

    [MaxLength(50)]
    public string Color { get; set; }
    //Variavel para atribuir a cor do carro

    [Required]
    public int Age { get; set; }
    //Variavel para atribuir o ano do carro

    public string CarPlate { get; set; }
    //Variavel para atribuir a placa do carro

    public List<ImageCar> Images { get; set; }
    //Variavel para criar uma lista onde fica as imagens //instancia uma lista na hora que a entidade for criada

    public Carros()
    {
        
    }
    public Carros(string carBrand, string model, string color, int age, string carPlate)
    {
        IdCar = Guid.NewGuid();
        CarBrand = carBrand;
        Model = model;
        Color = color;
        Age = age;
        CarPlate = carPlate;
        Images = new List<ImageCar>();
    }
}
