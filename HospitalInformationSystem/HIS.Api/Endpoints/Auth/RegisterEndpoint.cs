using HIS.Api.Mappers;
using HIS.Application.Services.Auth;
using HIS.Contracts.Requests.Auth;

namespace HIS.Api.Endpoints.Auth
{
    public static class RegisterEndpoint
    {
        public const string Name = "Register";

        public static IEndpointRouteBuilder MapRegister(this IEndpointRouteBuilder builder)
        {
            builder.MapPost(ApiEndpoints.Auth.Register,
                async (UserRequest request, IAuthService authService, CancellationToken token) =>
                {
                    var user = request.MapToRegisterUser();

                    var loggedUser = await authService.RegisterUserAsync(user, token);

                    var response = user.MapToResponse();

                    return TypedResults.Ok(response);
                })
                .WithName(Name);

            return builder;
        }
    }
}
