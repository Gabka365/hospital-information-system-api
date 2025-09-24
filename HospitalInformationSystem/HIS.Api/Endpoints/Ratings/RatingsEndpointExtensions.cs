using System.Runtime.CompilerServices;

namespace HIS.Api.Endpoints.Ratings
{
    public static class RatingsEndpointExtensions
    {
         public static IEndpointRouteBuilder AddRatingsEndpoints(this IEndpointRouteBuilder builder)
         {
            builder.MapRateDoctor();
            builder.MapDeleteRating();
            builder.MapGetUserRatings();
            return builder;
         }
    }
}
