using HIS.Api.Auth;
using HIS.Application.Services.Doctors;

namespace HIS.Api.Endpoints.Doctors
{
    public static class DeleteDoctorEndpoint
    {
        public const string Name = "DeleteDoctor";

        public static IEndpointRouteBuilder MapDeleteDoctor(this IEndpointRouteBuilder builder)
        {
            builder.MapDelete(ApiEndpoints.V1.Doctors.Delete,
                async (Guid id, IDoctorService doctorService,
                HttpContext context, CancellationToken token) =>
                {
                    var userId = context.GetUserId();
                    var isDeleted = await doctorService.DeleteDoctorAsync(id, userId, token);

                    if (!isDeleted)
                    {
                        return Results.NotFound();
                    }

                    return TypedResults.Ok();
                })
                .WithName(Name)
                .RequireAuthorization(AuthConstants.AdminPolicy);

            return builder;
        }
    }
}
