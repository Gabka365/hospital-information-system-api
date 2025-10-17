using System.Runtime.CompilerServices;

namespace HIS.Api.Endpoints.Doctors
{
    public static class DoctorsEndpointExtensions
    {
         public static IEndpointRouteBuilder AddDoctorsEndpoints(this IEndpointRouteBuilder builder)
         {
            builder.MapGetDoctor();
            builder.MapCreateDoctor();
            builder.MapGetAllDoctors();
            builder.MapUpdateDoctor();
            builder.MapDeleteDoctor();
            builder.MapGetDoctorsPatients();
            builder.MapAddPatientForDoctor();
            builder.MapAddPatientForCurrentUser();
            builder.MapDeleteDoctorPatient();

            return builder;
         }
    }
}
