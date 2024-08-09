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

        CarrosEndPointComId.MapPut("", CarsHandlers.PutCar);

        CarrosEndPointComId.MapDelete("", CarsHandlers.DeleteCar);
    }

    public static void RegisterUsuariosEndPoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var UserEndPoint = endpointRouteBuilder.MapGroup("/Users");

        var UserWithId = UserEndPoint.MapGroup("/{Id:guid}");

        UserEndPoint.MapGet("", UserHandlers.GetUsers);

        UserWithId.MapGet("", UserHandlers.GetUsersById).WithName("GetUserId");

        UserEndPoint.MapPost("", UserHandlers.PostUser);

        UserWithId.MapDelete("", UserHandlers.DeleteUser);

        UserWithId.MapPut("", UserHandlers.PutUser);

    }
}
