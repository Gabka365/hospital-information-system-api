using HIS.Api.Endpoints.Doctors;
using HIS.Api.Endpoints.Patients;
using HIS.Api.Endpoints.Ratings;

namespace HIS.Api.Endpoints
{
    public static class EndpointsExtensions
    {
        public static IEndpointRouteBuilder AddApiEndpoints(this IEndpointRouteBuilder builder)
        {
            builder.AddDoctorsEndpoints();
            builder.AddPatientsEndpoints();
            builder.AddAuthEndpoints();
            builder.AddRatingsEndpoints(); 
            return builder;  
        }
    }
}
