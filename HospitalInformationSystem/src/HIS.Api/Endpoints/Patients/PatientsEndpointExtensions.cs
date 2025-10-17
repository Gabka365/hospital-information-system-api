using System.Runtime.CompilerServices;

namespace HIS.Api.Endpoints.Patients
{
    public static class PatientsEndpointExtensions
    {
         public static IEndpointRouteBuilder AddPatientsEndpoints(this IEndpointRouteBuilder builder)
         {
            builder.MapCreatePatient();
            builder.MapGetPatient();
            builder.MapGetAllPatients();
            builder.MapUpdatePatient();
            builder.MapDeletePatient();
            builder.MapAddDoctorForCurrentUser();
            builder.MapAddDoctorForPatient();
            builder.MapGetPatientsDoctors();
            builder.MapDeletePatientDoctor();

            return builder;
         }
    }
}
