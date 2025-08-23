using HIS.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Application.Services.Ratings
{
    public interface IRatingsService
    {
        Task<bool> RateDoctorAsync(Guid doctorId, int rating, Guid userId, CancellationToken token = default);

        Task<bool> DeleteRatingAsync(Guid doctorId, Guid userId, CancellationToken token = default);

        Task<IEnumerable<DoctorRating>> GetRatingsForUserAsync(Guid userId, CancellationToken token = default);
    }
}
