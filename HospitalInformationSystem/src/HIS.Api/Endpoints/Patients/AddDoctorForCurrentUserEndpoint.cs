using HIS.Api.Auth;
using HIS.Application.Services.Patients;
using HIS.Contracts.Responses.Patients;

namespace HIS.Api.Endpoints.Patients
{
    public static class AddDoctorForCurrentUserEndpoint
    {
        public const string Name = "AddDoctorForCurrentUser";

        public static IEndpointRouteBuilder MapAddDoctorForCurrentUser(this IEndpointRouteBuilder builder)
        {
            builder.MapGet(ApiEndpoints.Patients.AddDoctorForCurrentUser, async (Guid doctorId,
                IPatientService patientService, HttpContext context, CancellationToken token) =>
            {
                var userId = context.GetUserId();

                var result = await patientService.AddDoctorForPatientAsync(doctorId, userId, token);

                if (!result)
                {
                    return Results.BadRequest();
                }

                return TypedResults.Ok(result);
            })
            .Produces<bool>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithName($"{Name}V2")
            .WithApiVersionSet(ApiVersioning.VersionSet)
            .HasApiVersion(1.0);

            return builder;
        }
    }
}
