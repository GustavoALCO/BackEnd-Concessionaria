namespace Concessionaria.Models.Moto;
public class MotoWithNumberDTO
{
    //Classe Moto
    public Guid IdMoto { get; set; }
    public string MotoBrand { get; set; }
    public string Model { get; set; }
    public string Fuel { get; set; }
    public string Color { get; set; }
    public string Plate { get; set; }
    public int Age { get; set; }
    public int Km { get; set; }
    public int Price { get; set; }
    public List<string> Url { get; set; } // URLs de imagens


    //Classe Auditable 
    public DateTimeOffset DateUpload { get; set; } // Data quando a moto é criada
    public Guid CreateUserId { get; set; } // ID do usuário que criou o anuncio
    public DateTimeOffset DateUpdate { get; set; } // Data quando a moto é alterada pela ultima vez
    public Guid AlterationUserId { get; set; } // ID do usuário que fez a última alteração

    //Classe Store 
    public int IdStore { get; set; }
    public string[] PhoneNumbers { get; set; }
}

