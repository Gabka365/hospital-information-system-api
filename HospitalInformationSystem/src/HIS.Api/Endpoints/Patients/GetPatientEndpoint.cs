using HIS.Api.Auth;
using HIS.Api.Mappers;
using HIS.Application.Services.Doctors;
using HIS.Application.Services.Patients;
using HIS.Contracts.Responses.Patients;
using Microsoft.AspNetCore.Mvc;

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
                    var patient = await patientService.GetPatientAsync(id, token);

                    if (patient is null)
                    {
                        return Results.NotFound();
                    }

                    return TypedResults.Ok(patient.MapToResponse());
                })
                .Produces<PatientResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .WithName($"{Name}V1")
                .WithApiVersionSet(ApiVersioning.VersionSet)
                .HasApiVersion(1.0)
                .ReportApiVersions()
                //.RequireAuthorization()
                .WithMetadata(new ResponseCacheAttribute
                {
                    Duration = 30,
                    VaryByHeader = "Accept, Accept-Encoding",
                    Location = ResponseCacheLocation.Client
                });


            return builder;
        }
    }
}
