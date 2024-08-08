namespace Concessionaria.Models;

public class ImageDTO
{
    public Guid IdCar { get; set; }
    public required Guid IdImage { get; set; }
    //Adiciona um Id unico para cada imagem que fizer upload
    public required List<string> Url { get; set; } = new List<string>();

    public required DateTimeOffset DateUpload { get; set; }

    public DateTimeOffset? DateUpdate { get; set; }

    
}
