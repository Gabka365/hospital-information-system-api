using HIS.Application.Models;
using HIS.Application.Repositories.Ratings;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Application.Services.Ratings
{
    public class RatingsService : IRatingsService
    {
        private readonly IRatingsRepository _ratingsRepository;

        public RatingsService(IRatingsRepository ratingsRepository)
        {
            _ratingsRepository = ratingsRepository;
        }

        public Task<bool> DeleteRatingAsync(Guid doctorId, Guid userId, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DoctorRating>> GetRatingsRotUserAsync(Guid userId, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RateDoctorAsync(Guid doctorId, int rating, Guid iuserId, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
