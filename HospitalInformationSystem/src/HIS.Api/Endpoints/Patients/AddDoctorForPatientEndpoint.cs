using HIS.Api.Auth;
using HIS.Application.Services.Patients;
using HIS.Contracts.Responses.Patients;

namespace HIS.Api.Endpoints.Patients
{
    public static class AddDoctorForPatientEndpoint
    {
        public const string Name = "AddDoctorForPatient";

        public static IEndpointRouteBuilder MapAddDoctorForPatient(this IEndpointRouteBuilder builder)
        {
            builder.MapGet(ApiEndpoints.Patients.AddDoctorForPatient, async (Guid DoctorId, Guid PatientId,
                IPatientService patientService, CancellationToken token) =>
            {
                var result = await patientService.AddDoctorForPatientAsync(DoctorId, PatientId, token);

                if (result == false)
                {
                    return Results.BadRequest();
                }

                return TypedResults.Ok(result);
            })
            .Produces<PatientsResponseWithoutPagination>(StatusCodes.Status200OK)
            .RequireAuthorization(AuthConstants.AdminPolicy)
            .WithName($"{Name}V2")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(2.0);

            return builder;
        }
    }
}
