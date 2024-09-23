using Concessionaria.Entities;
using Concessionaria.Service;


namespace Concessionaria.Models.Cars;

public class MotoCreateDTO
{
    public Guid IdMoto { get; set; }
    public string MotoBrand { get; set; }
    public string Model { get; set; }
    public string Description { get; set; }
    public string Fuel { get; set; }
    public string Color { get; set; }
    public int Age { get; set; }

    //Classe Auditable 
    public DateTimeOffset DateUpload { get; set; } // Data quando a moto é criada
    public Guid CreateUserId { get; set; } // ID do usuário que criou o anuncio

    //Classe Imagens
    public List<string> Url { get; set; } // URLs de imagens

    public MotoCreateDTO()
    {
        IdMoto = Guid.NewGuid();
        DateUpload = DateTimeOffset.Now;
    }//construtor para adicionar hora em que foi Criado 
}
