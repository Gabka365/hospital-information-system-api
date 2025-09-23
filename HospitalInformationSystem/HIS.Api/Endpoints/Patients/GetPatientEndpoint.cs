using HIS.Api.Auth;
using HIS.Api.Mappers;
using HIS.Application.Services.Doctors;
using HIS.Application.Services.Patients;
using HIS.Contracts.Responses.Patients;

namespace HIS.Api.Endpoints.Patients
{
    public static class GetPatientEndpoint
    {
        public const string Name = "GetPatient";

        public static IEndpointRouteBuilder MapGetPatient(this IEndpointRouteBuilder builder)
        {
            builder.MapGet(ApiEndpoints.Patients.Get,
                async (Guid id, IPatientService patientService,
                HttpContext context, CancellationToken token) =>
                {
                    var doctor = await patientService.GetPatientAsync(id, token);

                    if (doctor is null)
                    {
                        return Results.NotFound();
                    }

                    return TypedResults.Ok(doctor.MapToResponse());
                })
                .Produces<PatientResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .WithName(Name);


            return builder;
        }
    }
}
