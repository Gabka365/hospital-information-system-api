using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Contracts.Responses.Ratings
{
    public class RatingsResponse
    {
        public required IEnumerable<RatingResponse> RatingResponses { get; init; }
    }


    public class RatingResponse
    {
        public required Guid DoctorId { get; init; }
        public required string FirstName { get; init; }
        public required string LastName { get; init; }
        public required string Surname { get; init; }
        public required int Rating { get; init; }
    }
}
