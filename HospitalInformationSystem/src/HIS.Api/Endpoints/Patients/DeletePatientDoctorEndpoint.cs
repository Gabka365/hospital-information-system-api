using HIS.Api.Auth;
using HIS.Application.Services.Patients;
using Microsoft.AspNetCore.Mvc;

namespace HIS.Api.Endpoints.Patients
{
    public static class DeletePatientDoctorEndpoint
    {
        public const string Name = "DeletePatientDoctor";

        public static IEndpointRouteBuilder MapDeletePatientDoctor(this IEndpointRouteBuilder builder)
        {
            builder.MapDelete(ApiEndpoints.Patients.DeletePatientDoctor,
                async (Guid DoctorId, Guid PatientId, IPatientService patientService,
                HttpContext context, CancellationToken token) =>
                {
                    var isDeleted = await patientService.DeletePatientDoctorAsync(DoctorId, PatientId, token);

                    if (!isDeleted)
                    {
                        return Results.NotFound();
                    }

                    return TypedResults.Ok();
                })
                .Produces<bool>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .RequireAuthorization(AuthConstants.AdminPolicy)
                .WithName($"{Name}V1")
                .WithApiVersionSet(ApiVersioning.VersionSet)
                .HasApiVersion(2.0)
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
