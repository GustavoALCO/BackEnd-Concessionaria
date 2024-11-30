using Azure.Storage.Blobs;
using System.Text.RegularExpressions;

namespace Concessionaria.Service;

public class ImageUpload
{
    private  readonly string _connectionString;

    public ImageUpload(IConfiguration configuration)
    {
        _connectionString = configuration["AzureBlobStorage:ConnectionString"];
        //pegando a connectionstring do appsettings

        if (string.IsNullOrWhiteSpace(_connectionString))
            throw new ArgumentException(nameof(_connectionString), "Não foi possivel conectar no azure blob");
    }
    public async Task<string[]> UploadBase64ImagesAsync(string[] base64Images)
    {
        List<string> imageUrls = new List<string>();

        foreach (var base64Image in base64Images)
        {
            var fileName = $"{Guid.NewGuid().ToString()}.jpg";
            // Gera um nome único para a imagem

            var data = new Regex(@"^data:image\/[a-z]+;base64,").Replace(base64Image, "");
            // Remove a parte desnecessária do base64

            byte[] imageBytes = Convert.FromBase64String(data);
            // Converte a string base64 em um array de bytes

            var blobClient = new BlobClient(_connectionString, "carros", fileName);
            // Cria um cliente para o Blob Storage

            // Envia a imagem para o Storage de forma assíncrona
            using (var stream = new MemoryStream(imageBytes))
            {
                await blobClient.UploadAsync(stream);
            }

            imageUrls.Add(blobClient.Uri.AbsoluteUri); // Adiciona a URL da imagem à lista
        }

        return imageUrls.ToArray();
        // Retorna a lista de URLs das imagens
    }

    public async Task DeleteUrlImage(string[] urls, string Container)
    {
        try
        {
            foreach(var url in urls){

               Uri uri = new Uri(url);

                var deleteBlob = new BlobClient(_connectionString, Container, uri.Segments[^1]);

                await deleteBlob.DeleteAsync();
            }
        }
        catch (Exception ex)
        {
           Console.WriteLine($"Não foi Possivel Deletar a imagem erro? {ex}");
            return;
        }
    }
}
