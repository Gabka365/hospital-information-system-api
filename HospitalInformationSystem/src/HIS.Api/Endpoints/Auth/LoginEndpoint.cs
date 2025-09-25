using HIS.Api.Auth;
using HIS.Api.Mappers;
using HIS.Application.Services.Auth;
using HIS.Application.Services.Doctors;
using HIS.Contracts.Requests.Auth;
using HIS.Contracts.Requests.Doctors;
using HIS.Contracts.Responses.Auth;
using Microsoft.AspNetCore.OutputCaching;

namespace HIS.Api.Endpoints.Auth
{
    public static class LoginEndpoint
    {
        public const string Name = "Login";

        public static IEndpointRouteBuilder MapLogin(this IEndpointRouteBuilder builder)
        {
            builder.MapPost(ApiEndpoints.Auth.Login,
                async (UserRequest request, IAuthService authService, CancellationToken token) =>
                {
                    var customClaims = request.CustomClaims;
                    var user = request.MapToLoggedUser();

                    var loggedUser = await authService.LoginUserAsync(user, customClaims, token);

                    if (loggedUser == null)
                    {
                        return Results.BadRequest("Try another one.");
                    }

                    var response = loggedUser!.MapToResponse();

                    return TypedResults.Ok(response);
                })
                .Produces<UserResponse>(StatusCodes.Status200OK)
                .WithName($"{Name}V1")
                .WithApiVersionSet(ApiVersioning.VersionSet)
                .HasApiVersion(1.0);


            return builder;
        }
    }
}
