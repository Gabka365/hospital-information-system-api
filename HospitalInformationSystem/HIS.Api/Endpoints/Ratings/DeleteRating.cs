using HIS.Api.Auth;
using HIS.Application.Services.Ratings;
using HIS.Contracts.Requests.Ratings;
using Microsoft.AspNetCore.Mvc;

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
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .WithName($"{Name}V1")
                .WithApiVersionSet(ApiVersioning.VersionSet)
                .HasApiVersion(1.0)
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
