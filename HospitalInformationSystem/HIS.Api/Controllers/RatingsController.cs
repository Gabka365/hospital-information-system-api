using HIS.Api.Auth;
using HIS.Application.Services.Ratings;
using HIS.Contracts.Requests.Ratings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HIS.Api.Controllers
{
    public class RatingsController : ControllerBase
    {
        private readonly IRatingService _ratingService;

        public RatingsController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        [Authorize]
        [HttpPut(ApiEndpoints.Doctors.Rate)]
        public async Task<IActionResult> RateDoctor([FromRoute] Guid id, [FromBody] RateDoctorRequest request, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        [HttpPut(ApiEndpoints.Ratings.DeleteRatings)]
        public async Task<IActionResult> DeleteRating([FromRoute] Guid id, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        [HttpPut(ApiEndpoints.Ratings.GetUserRatings)]
        public async Task<IActionResult> GetUserRatings(CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

    }
}
