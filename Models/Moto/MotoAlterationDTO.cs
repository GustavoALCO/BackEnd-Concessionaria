

namespace Concessionaria.Models.Cars;

public class MotoAlterationDTO
{
    public string MotoBrand { get; set; }
    public string Model { get; set; }
    public string Description { get; set; }
    public string Fuel { get; set; }
    public string Color { get; set; }
    public int Age { get; set; }

    //Classe Auditable 
    public DateTimeOffset? DateUpdate { get; set; } // Data quando a moto é alterada pela ultima vez
    public Guid? AlterationUserId { get; set; } // ID do usuário que fez a última alteração

    //Classe Imagens
    public List<string> Url { get; set; } // URLs de imagens

    public MotoAlterationDTO()
    {
        DateUpdate = DateTimeOffset.Now;
    }//construtor para adicionar hora em que foi alterado pela ultima vez 
}
