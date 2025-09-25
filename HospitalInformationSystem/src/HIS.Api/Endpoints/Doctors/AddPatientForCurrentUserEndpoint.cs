using HIS.Api.Auth;
using HIS.Application.Services.Doctors;

namespace HIS.Api.Endpoints.Doctors
{
    public static class AddPatientForCurrentUserEndpoint
    {
        public const string Name = "AddPatientForCurrentUser";

        public static IEndpointRouteBuilder MapAddPatientForCurrentUser(this IEndpointRouteBuilder builder)
        {
            builder.MapGet(ApiEndpoints.V2.Doctors.AddPatientForCurrentUser, async (Guid patientId,
                IDoctorService doctorService, HttpContext context, CancellationToken token) =>
            {
                var userId = context.GetUserId();

                var result = await doctorService.AddPatientForDoctorAsync(patientId, userId, token);

                if (!result)
                {
                    return Results.BadRequest();
                }

                return TypedResults.Ok(result);
            })
            .Produces<bool>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithName($"{Name}V2")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(2.0);

            return builder;
        }
    }
}
