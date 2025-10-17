using HIS.Api.Mappers;
using HIS.Application.Services.Doctors;
using HIS.Contracts.Responses.Patients;

namespace HIS.Api.Endpoints.Doctors
{
    public static class GetDoctorsPatientsEndpoint
    {
        public const string Name = "GetDoctorsPatients";

        public static IEndpointRouteBuilder MapGetDoctorsPatients(this IEndpointRouteBuilder builder)
        {
            builder.MapGet(ApiEndpoints.V2.Doctors.GetDoctorsPatients, async (Guid id, 
                IDoctorService doctorService, CancellationToken token) =>
            {
                var result = await doctorService.GetDoctorsPatientsAsync(id, token);

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
