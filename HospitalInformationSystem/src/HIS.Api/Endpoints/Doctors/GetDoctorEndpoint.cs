using HIS.Api.Auth;
using HIS.Api.Mappers;
using HIS.Application.Services.Doctors;
using HIS.Contracts.Responses.Doctors;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace HIS.Api.Endpoints.Doctors
{
    public static class GetDoctorEndpoint
    {
        public const string Name = "GetDoctor";

        public static IEndpointRouteBuilder MapGetDoctor(this IEndpointRouteBuilder builder)
        {
            builder.MapGet(ApiEndpoints.V1.Doctors.Get,
                async (Guid id, IDoctorService doctorService,
                HttpContext context, CancellationToken token) =>
                {
                    var userId = context.GetUserId();
                    var doctor = await doctorService.GetDoctorByIdAsync(id, userId, token);

                    if (doctor is null)
                    {
                        return Results.NotFound();
                    }
                    return TypedResults.Ok(doctor.MapToResponse());
                })
                .Produces<DoctorResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .WithName($"{Name}V1")
                .WithApiVersionSet(ApiVersioning.VersionSet)
                .HasApiVersion(1.0)
                .ReportApiVersions()
                .RequireAuthorization()
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
