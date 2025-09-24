using HIS.Api.Auth;
using HIS.Application.Services.Doctors;

namespace HIS.Api.Endpoints.Doctors
{
    public static class AddPatientForDoctorEndpoint
    {
        public const string Name = "AddPatientForDoctor";

        public static IEndpointRouteBuilder MapAddPatientForDoctor(this IEndpointRouteBuilder builder)
        {
            builder.MapGet(ApiEndpoints.V2.Doctors.AddPatientForDoctor, async (Guid patientId, Guid doctorId,
                IDoctorService doctorService, CancellationToken token) =>
            {
                var result = await doctorService.AddPatientForDoctorAsync(patientId, doctorId, token);

                if (!result)
                {
                    return Results.BadRequest();
                }

                return TypedResults.Ok(result);
            })
            .Produces<bool>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .RequireAuthorization(AuthConstants.AdminPolicy)
            .WithName($"{Name}V2")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(2.0);

            return builder;
        }
    }
}
