using HIS.Api.Auth;
using HIS.Api.Mappers;
using HIS.Application.Models;
using HIS.Application.Services.Ratings;

namespace HIS.Api.Endpoints.Ratings
{
    public static class GetUserRatings
    {
        public const string Name = "GetUserRatings";

        public static IEndpointRouteBuilder MapGetUserRatings(this IEndpointRouteBuilder builder)
        {
            builder.MapGet(ApiEndpoints.V1.Doctors.GetUserRatingsForDoctor,
                async (IRatingsService ratingsService,
                HttpContext context, CancellationToken token) =>
                {
                    var userId = context.GetUserId();

                    var userRatings = await ratingsService.GetRatingsForUserAsync(userId, token);

                    var response = userRatings.MapToRatingsResponse();

                    return TypedResults.Ok(response);
                })
                .Produces<IEnumerable<DoctorRating>>(StatusCodes.Status200OK)
                .WithName(Name);

            return builder;
        }
    }
}
