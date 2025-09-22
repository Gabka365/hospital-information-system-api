using HIS.Api.Auth;
using HIS.Application.Services.Ratings;
using HIS.Contracts.Requests.Ratings;

namespace HIS.Api.Endpoints.Ratings
{
    public static class DeleteRating
    {
        public const string Name = "DeleteRating";

        public static IEndpointRouteBuilder MapDeleteRating(this IEndpointRouteBuilder builder)
        {
            builder.MapDelete(ApiEndpoints.V1.Doctors.DeleteRating,
                async (Guid id, IRatingsService ratingsService,
                HttpContext context, CancellationToken token) =>
                {
                    var userId = context.GetUserId();

                    var isRated = await ratingsService.DeleteRatingAsync(id, userId, token);

                    return isRated ? Results.Ok() : Results.NotFound();
                })
                .WithName(Name);

            return builder;
        }
    }
}
