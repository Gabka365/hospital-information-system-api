using HIS.Api.Auth;
using HIS.Application.Services.Patients;
using HIS.Application.Services.Ratings;
using HIS.Contracts.Requests.Ratings;
using HIS.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;

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
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .Produces<ValidationErrorResponse>(StatusCodes.Status404NotFound)
                .WithName($"{Name}V1")
                .WithApiVersionSet(ApiVersioning.VersionSet)
                .HasApiVersion(1.0)
                .WithMetadata(new ResponseCacheAttribute
                {
                    Duration = 30,
                    VaryByQueryKeys = new[] { "Rating" },
                    Location = ResponseCacheLocation.Client
                });

            return builder;
        }
    }
}
