using System.Runtime.CompilerServices;

namespace HIS.Api.Endpoints.Doctors
{
    public static class AuthEndpointExtensions
    {
         public static IEndpointRouteBuilder AddDoctorsEndpoints(this IEndpointRouteBuilder builder)
         {
            //builder.MapCreateDoctor();
            //builder.MapGetDoctor();
            //builder.MapGetAllDoctors();
            //builder.MapUpdateDoctor();
            //builder.MapDeleteDoctor();
            return builder;
         }
    }
}
