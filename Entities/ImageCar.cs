namespace Concessionaria.Entities;

public class ImageCar
{
    public required Guid IdImage { get; set; }
    //Adiciona um Id unico para cada imagem que fizer upload
    public required List<string> Url { get; set; } = new List<string>();

    public required DateTimeOffset DateUpload { get; set; }

    public  DateTimeOffset? DateUpdate { get; set; }

    public Guid IdCar { get; set; }

    public Carros Carros { get; set; }

    public ImageCar()
    {
        
    }
    public ImageCar(List<string> url)
    {
        IdImage = Guid.NewGuid();
        Url = url;
        DateUpload = DateTimeOffset.Now;
    }
}

