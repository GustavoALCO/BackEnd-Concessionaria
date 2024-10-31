using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Concessionaria.Entities;

[NotMapped] //informa ao EF que a classe não sera mapeada como uma tabela
public class Auditable 
{
    public DateTimeOffset DateUpload { get; set; }

    // Variavel para o usuário que criou
    public Guid CreateUserId { get; set; }

    public DateTimeOffset? DateUpdate { get; set; }

    // Variavel para o usuário que alterou
    public Guid AlterationUserId { get; set; }

}
