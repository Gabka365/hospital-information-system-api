using HIS.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Application.Repositories.Ratings
{
    public class RatingRepository : IRatingRepository
    {
        public Task<bool> DeleteRatingAsync(Guid doctorId, Guid userId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<float?> GetRatingAsync(Guid doctorId, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<(float? Rating, int? UserRating)> GetRatingAsync(Guid doctorId, Guid userId, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DoctorRating>> GetRatingsForUserAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RateDoctorAsync(Guid doctorId, int rating, Guid userId, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
