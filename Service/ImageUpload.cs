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
            throw new ArgumentException(nameof(_connectionString), "Não foi possivel conectar no azure blob por a variavel estar nula");
    }
    public async Task<List<string>> UploadBase64ImagesAsync(Guid ID, List<string> base64Images, string Container)
    {
        List<string> imageUrls = new List<string>();

        foreach (var base64Image in base64Images)
        {
            var fileName = $"{ID.ToString()}&{Guid.NewGuid().ToString()}.jpg";
            // Adiciona o nome a uma imagem

            var data = new Regex(@"^data:image\/[a-z]+;base64,").Replace(base64Image, "");
            // Remove a primeira parte do base64 que é desnecessária

            byte[] imageBytes = Convert.FromBase64String(data);
            // Transforma a imagem em um array de bytes

            var blobClient = new BlobClient(_connectionString, Container, fileName);
            // Define o Storage no qual a imagem será armazenada

            // Envia a imagem para o Storage de forma assíncrona
            using (var stream = new MemoryStream(imageBytes))
            {
                await blobClient.UploadAsync(stream);
            }

            imageUrls.Add(blobClient.Uri.AbsoluteUri);
            // Adiciona a URL da imagem na lista
        }

        return imageUrls;
        // Retorna a lista de URLs das imagens
    }
}
