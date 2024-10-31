

namespace Concessionaria.Models.Cars;

public class MotoAlterationDTO
{
    public string MotoBrand { get; set; }
    public string Model { get; set; }
    public string Fuel { get; set; }
    public string Color { get; set; }
    public string Plate { get; set; }
    public int Age { get; set; }
    public int Km { get; set; }
    public int Price { get; set; }

    public string[] Url { get; set; }

    //Classe Auditable 
    public DateTimeOffset DateUpdate { get; set; } // Data quando a moto é alterada pela ultima vez
    public Guid AlterationUserId { get; set; } // ID do usuário que fez a última alteração

    public int IdStore { get; set; }

}
