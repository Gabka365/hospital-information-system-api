using HIS.Api.Auth;
using HIS.Application.Services.Patients;
using HIS.Application.Services.Ratings;
using HIS.Contracts.Requests.Ratings;

namespace HIS.Api.Endpoints.Ratings
{
    public static class RateDoctor
    {
        public const string Name = "RateDoctor";

        public static IEndpointRouteBuilder MapRateDoctor(this IEndpointRouteBuilder builder)
        {
            builder.MapPut(ApiEndpoints.V1.Doctors.Rate,
                async (Guid id, RateDoctorRequest request, IRatingsService ratingsService,
                HttpContext context, CancellationToken token) =>
                {
                    var userId = context.GetUserId();

                    var isRated = await ratingsService.RateDoctorAsync(id, request.Rating, userId, token);

                    return isRated ? Results.Ok() : Results.NotFound();

                })
                .WithName(Name);

            return builder;
        }
    }
}
