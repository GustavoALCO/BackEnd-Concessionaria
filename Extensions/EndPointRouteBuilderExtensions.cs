

using Concessionaria.EndpointHandlers;

namespace Concessionaria.Extensions;

public static class EndPointRouteBuilderExtensions
{
    public static void RegisterCarrosEndpoints(this IEndpointRouteBuilder endpointRouteBuilder ) 
    {                                                              //Pega a rota que esta a URl
        var CarrosEndPoint = endpointRouteBuilder.MapGroup("/Motos");

        var CarrosEndPointComId = CarrosEndPoint.MapGroup("/{Id:Guid}");

        CarrosEndPoint.MapGet("", MotoHandlers.GetMotos).WithName("GetCarroId").WithSummary("Busca apenas pelo o nome ou retorna todos");

        CarrosEndPointComId.MapGet("", MotoHandlers.GetmotoId).WithSummary("Busca o carro pelo Id selecionado");

        CarrosEndPoint.MapGet("Filtro", MotoHandlers.GetmotoFilter).WithSummary("Faz uma busca com todos os Filtros passado, Não precisa do token JWT");

        CarrosEndPoint.MapPost("", MotoHandlers.Postmoto).WithSummary("Publica Um carro, Sendo necessario um token");

        CarrosEndPointComId.MapPut("", MotoHandlers.PutMoto).WithSummary("Altera Propriedades do carro, É necessario um token");

        CarrosEndPointComId.MapDelete("", MotoHandlers.DeleteMoto).WithSummary("Deleta os carros sendo necessario um token JWT");
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

    public static void RegisterStoreEndPoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var StoreEndpoint = endpointRouteBuilder.MapGroup("/Store");

        var StoreWithId = StoreEndpoint.MapGroup("{Id:int}");

        StoreWithId.MapGet("", StoreHandlers.GetStoreAsync);

        StoreEndpoint.MapPost("", StoreHandlers.PostStoreAsync);

        StoreWithId.MapDelete("", StoreHandlers.DeleteStore);

        StoreWithId.MapPut("", StoreHandlers.PutStoreAsync);
    }
}
