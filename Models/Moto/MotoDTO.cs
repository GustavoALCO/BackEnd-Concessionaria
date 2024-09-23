
namespace Concessionaria.Models.Cars;

public class MotoDTO
{
    //Classe Moto
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
    public DateTimeOffset? DateUpdate { get; set; } // Data quando a moto é alterada pela ultima vez
    public Guid? AlterationUserId { get; set; } // ID do usuário que fez a última alteração

    //Classe Imagens
    public Guid IdImage { get; set; } //Id para o busca das imagens 
    public List<string> Url { get; set; } // URLs de imagens
}
