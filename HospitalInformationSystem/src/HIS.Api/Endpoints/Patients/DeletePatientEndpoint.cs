using HIS.Api.Auth;
using HIS.Application.Services.Doctors;
using HIS.Application.Services.Patients;
using Microsoft.AspNetCore.Mvc;

namespace HIS.Api.Endpoints.Patients
{
    public static class DeletePatientEndpoint
    {
        public const string Name = "DeletePatient";

        public static IEndpointRouteBuilder MapDeletePatient(this IEndpointRouteBuilder builder)
        {
            builder.MapDelete(ApiEndpoints.Patients.Delete,
                async (Guid id, IPatientService patientService,
                HttpContext context, CancellationToken token) =>
                {
                    var isDeleted = await patientService.DeletePatientAsync(id, token);

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
                .HasApiVersion(1.0)
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
