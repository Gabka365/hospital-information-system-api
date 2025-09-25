using HIS.Application.Models;
using HIS.Contracts.Responses.Ratings;

namespace HIS.Api.Mappers
{
    public static class RatingsMapping
    {
        public static RatingsResponse MapToRatingsResponse(this IEnumerable<DoctorRating> doctorRatings)
        {
            return new RatingsResponse
            {
                RatingResponses = doctorRatings.Select(r => new RatingResponse
                {
                    DoctorId = r.Id,
                    FirstName = r.FirstName,
                    LastName = r.LastName,
                    Surname = r.Surname,
                    Rating = r.Rating
                })
            };
        }
    }
}
