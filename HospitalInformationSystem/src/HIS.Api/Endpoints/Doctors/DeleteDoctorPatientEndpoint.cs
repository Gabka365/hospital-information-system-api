using HIS.Api.Auth;
using HIS.Application.Services.Doctors;
using HIS.Application.Services.Patients;
using Microsoft.AspNetCore.Mvc;

namespace HIS.Api.Endpoints.Doctors
{
    public static class DeleteDoctorPatientEndpoint
    {
        public const string Name = "DeleteDoctorPatient";

        public static IEndpointRouteBuilder MapDeleteDoctorPatient(this IEndpointRouteBuilder builder)
        {
            builder.MapDelete(ApiEndpoints.V2.Doctors.DeleteDoctorPatient,
                async (Guid DoctorId, Guid PatientId, IDoctorService doctorService,
                HttpContext context, CancellationToken token) =>
                {
                    var isDeleted = await doctorService.DeleteDoctorPatientAsync(DoctorId, PatientId, token);

                    if (!isDeleted)
                    {
                        return Results.NotFound();
                    }

                    return TypedResults.Ok();
                })
                .Produces<bool>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .RequireAuthorization(AuthConstants.AdminPolicy)
                .WithName($"{Name}V2")
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
