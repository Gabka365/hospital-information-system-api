using HIS.Api.Mappers;
using HIS.Application.Services.Patients;
using HIS.Contracts.Responses.Patients;

namespace HIS.Api.Endpoints.Patients
{
    public static class GetPatientsDoctorsEndpoint
    {
        public const string Name = "GetPatientsDoctors";

        public static IEndpointRouteBuilder MapGetPatientsDoctors(this IEndpointRouteBuilder builder)
        {
            builder.MapGet(ApiEndpoints.Patients.GetPatientsDoctors, async (Guid id, IPatientService patientService, CancellationToken token) =>
            {
                var result = await patientService.GetPatientsDoctorsAsync(id, token);

                var response = result.MapToResponses();

                return TypedResults.Ok(response);
            })
            .Produces<PatientsResponseWithoutPagination>(StatusCodes.Status200OK)
            .WithName($"{Name}V2")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(2.0);

            return builder;
        }
    }
}
