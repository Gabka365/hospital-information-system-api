using HIS.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Application.Repositories.Ratings
{
    public interface IRatingsRepository
    {
        Task<bool> RateDoctorAsync(Guid doctorId, int rating, Guid userId, CancellationToken token = default);

        Task<float?> GetRatingAsync(Guid doctorId, CancellationToken token = default);

        Task<(float? Rating, int? UserRating)> GetRatingAsync(Guid doctorId, Guid userId, CancellationToken token = default);

        Task<bool> DeleteRatingAsync(Guid doctorId, Guid userId, CancellationToken token = default);

        Task<IEnumerable<DoctorRating>> GetRatingsForUserAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
