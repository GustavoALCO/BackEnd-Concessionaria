using Concessionaria.EndpointHandlers;

namespace Concessionaria.Extensions;

public static class EndPointRouteBuilderExtensions
{
    public static void RegisterCarrosEndpoints(this IEndpointRouteBuilder endpointRouteBuilder ) 
    {                                                              //Pega a rota que esta a URl
        var CarrosEndPoint = endpointRouteBuilder.MapGroup("/Carros");

        var CarrosEndPointComId = CarrosEndPoint.MapGroup("/{IdCar:guid}");

        CarrosEndPoint.MapGet("", CarsHandlers.GetCarros).WithName("GetCarroId");

        CarrosEndPointComId.MapGet("", CarsHandlers.GetCarsId);

        CarrosEndPoint.MapPost("", CarsHandlers.PostCars);
    }
}
