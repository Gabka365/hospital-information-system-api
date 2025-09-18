using System.Runtime.CompilerServices;

namespace HIS.Api.Endpoints.Doctors
{
    public static class AuthEndpointExtensions
    {
         public static IEndpointRouteBuilder AddAuthEndpoints(this IEndpointRouteBuilder builder)
         {
            //builder.MapRegister();
            //builder.MapLogin();
            return builder;
         }
    }
}
