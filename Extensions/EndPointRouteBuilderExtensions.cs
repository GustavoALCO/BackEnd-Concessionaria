using Concessionaria.EndpointHandlers;

namespace Concessionaria.Extensions;

public static class EndPointRouteBuilderExtensions
{
    public static void RegisterCarrosEndpoints(this IEndpointRouteBuilder endpointRouteBuilder ) 
    {                                                              //Pega a rota que esta a URl
        var CarrosEndPoint = endpointRouteBuilder.MapGroup("/Carros");

        var CarrosEndPointComId = CarrosEndPoint.MapGroup("/{Id:Guid}");

        CarrosEndPoint.MapGet("", CarsHandlers.GetCarros).WithName("GetCarroId").WithSummary("Busca apenas pelo o nome ou retorna todos");

        CarrosEndPointComId.MapGet("", CarsHandlers.GetCarsId).WithSummary("Busca o carro pelo Id selecionado");

        CarrosEndPoint.MapGet("Filtro", CarsHandlers.GetCarsFilter).WithSummary("Faz uma busca com todos os Filtros passado, Não precisa do token JWT");

        CarrosEndPoint.MapPost("", CarsHandlers.PostCars).WithSummary("Publica Um carro, Sendo necessario um token");

        CarrosEndPointComId.MapPut("", CarsHandlers.PutCar).WithSummary("Altera Propriedades do carro, É necessario um token");

        CarrosEndPointComId.MapDelete("", CarsHandlers.DeleteCar).WithSummary("Deleta os carros sendo necessario um token JWT");
    }

    public static void RegisterUsuariosEndPoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var UserEndPoint = endpointRouteBuilder.MapGroup("/Users");

        var UserWithId = UserEndPoint.MapGroup("/{Id:Guid}");

        UserEndPoint.MapGet("", UserHandlers.GetUsers).WithSummary("Busca de todos os Usuarios");

        UserWithId.MapGet("", UserHandlers.GetUsersById).WithName("GetUserId").WithSummary("Busca de Usuario por ID especifico");

        UserEndPoint.MapPost("", UserHandlers.PostUser).WithSummary("Criação de usuario Não necessita do Token JWT");

        UserWithId.MapDelete("", UserHandlers.DeleteUser).WithSummary("exclusão de usuario necessita do Token JWT");

        UserWithId.MapPut("", UserHandlers.PutUser).WithSummary("Alteração de usuario necessita do Token JWT");

        endpointRouteBuilder.MapPost("/Login", UserHandlers.LoginRequest).WithSummary("Esta rota é de login irá retornar o Token Jwt"); 
    }
}
