

using Concessionaria.EndpointHandlers;

namespace Concessionaria.Extensions;

public static class EndPointRouteBuilderExtensions
{
    public static void RegisterCarrosEndpoints(this IEndpointRouteBuilder endpointRouteBuilder ) 
    {                                                              //Pega a rota que esta a URl
        var MotosEndpoints = endpointRouteBuilder.MapGroup("/Motos");

        var MotosEndPointComId = MotosEndpoints.MapGroup("/{Id:Guid}");

        MotosEndpoints.MapGet("", MotoHandlers.GetMotos).WithName("GetCarroId").WithSummary("Busca apenas pelo o nome ou retorna todos");

        MotosEndPointComId.MapGet("", MotoHandlers.GetmotoId).WithSummary("Busca o carro pelo Id selecionado");

        MotosEndpoints.MapGet("Destaque", MotoHandlers.GetNovasMotos).WithSummary("Busca as 10 ultimas motos Criadas");

        MotosEndpoints.MapGet("Filtro", MotoHandlers.GetmotoFilter).WithSummary("Faz uma busca com todos os Filtros passado, Não precisa do token JWT");

        MotosEndpoints.MapPost("", MotoHandlers.Postmoto).WithSummary("Publica Um carro, Sendo necessario um token").RequireAuthorization();

        MotosEndPointComId.MapPut("", MotoHandlers.PutMoto).WithSummary("Altera Propriedades do carro, É necessario um token").RequireAuthorization();

        MotosEndPointComId.MapDelete("", MotoHandlers.DeleteMoto).WithSummary("Deleta os carros sendo necessario um token JWT").RequireAuthorization();
    }

    public static void RegisterUsuariosEndPoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var UserEndPoint = endpointRouteBuilder.MapGroup("/Users");

        var UserWithId = UserEndPoint.MapGroup("/{Id:Guid}");

        UserEndPoint.MapGet("", UserHandlers.GetUsers).WithSummary("Busca de todos os Usuarios").RequireAuthorization();

        UserWithId.MapGet("", UserHandlers.GetUsersById).WithName("GetUserId").WithSummary("Busca de Usuario por ID especifico").RequireAuthorization();

        UserEndPoint.MapPost("", UserHandlers.PostUser).WithSummary("Criação de usuario Não necessita do Token JWT").RequireAuthorization();

        UserWithId.MapDelete("", UserHandlers.DeleteUser).WithSummary("exclusão de usuario necessita do Token JWT").RequireAuthorization();

        UserWithId.MapPut("", UserHandlers.PutUser).WithSummary("Alteração de usuario necessita do Token JWT").RequireAuthorization();

        UserEndPoint.MapPut("Alter/{Id:Guid}", UserHandlers.AlterPassword).WithSummary("AlterarSenha");

        endpointRouteBuilder.MapPost("/Login", UserHandlers.LoginRequest).WithSummary("Esta rota é de login irá retornar o Token Jwt");

        endpointRouteBuilder.MapGet("/VerifyToken", UserHandlers.VerifyToken).WithSummary("Rota para a verificação de Token JWT necessario possuir um").RequireAuthorization();

    }

    public static void RegisterStoreEndPoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var StoreEndpoint = endpointRouteBuilder.MapGroup("/Store");

        var StoreWithId = StoreEndpoint.MapGroup("{Id:int}");

        StoreEndpoint.MapGet("", StoreHandlers.GetStoreAsync).RequireAuthorization();

        StoreWithId.MapGet("", StoreHandlers.GetStoreId).RequireAuthorization();

        StoreEndpoint.MapPost("", StoreHandlers.PostStoreAsync).RequireAuthorization();

        StoreWithId.MapDelete("", StoreHandlers.DeleteStore).RequireAuthorization();

        StoreWithId.MapPut("", StoreHandlers.PutStoreAsync).RequireAuthorization();
    }
}
