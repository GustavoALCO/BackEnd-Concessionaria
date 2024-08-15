using System.ComponentModel.DataAnnotations;

namespace Concessionaria.Entities;

public class Cars
{
    [Key]
    [Required]
    public Guid IdCar { get; set; }
    //Id Gerado com GUID para ter pouca chance de se repetir
    public Guid IdUser { get; set; }
    // Chave estrangeira para Usuario
    public User User { get; set; }
    // Propriedade de navegação para Usuario
    
    public string CarBrand { get; set; }
    //Variavel para atribuir o nome da fabricante do carro
    
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

    public  List<string> Url { get; set; } = new List<string>();

    public  DateTimeOffset DateUpload { get; set; }

    public DateTimeOffset? DateUpdate { get; set; }
    public Cars()
    {
        
    }
    public Cars(string carBrand, string model, string color, int age, string carPlate)
    {
        IdCar = Guid.NewGuid();
        Url = new List<string>();
        CarBrand = carBrand;
        Model = model;
        Color = color;
        Age = age;
        CarPlate = carPlate;
        DateUpload = DateTimeOffset.Now;
    }
}
