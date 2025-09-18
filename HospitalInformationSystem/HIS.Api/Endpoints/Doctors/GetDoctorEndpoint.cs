using HIS.Api.Auth;
using HIS.Api.Mappers;
using HIS.Application.Services.Doctors;
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
            });
            return builder;
        }
    }
}
