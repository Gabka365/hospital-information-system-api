using HIS.Api.Mappers;
using HIS.Application.Services.Doctors;

namespace HIS.Api.Endpoints.Doctors
{
    public static class GetDoctorsPatientsEndpoint
    {
        public const string Name = "GetDoctor";

        public static IEndpointRouteBuilder MapGetDoctorsPatients(this IEndpointRouteBuilder builder)
        {
            builder.MapGet(ApiEndpoints.V2.Doctors.GetDoctorsPatients, async (Guid id, 
                IDoctorService doctorService, CancellationToken token) =>
            {
                var result = await doctorService.GetDoctorsPatientsAsync(id, token);

                var response = result.MapToResponses();

                return TypedResults.Ok(response);
            });

            return builder;
        }
    }
}
