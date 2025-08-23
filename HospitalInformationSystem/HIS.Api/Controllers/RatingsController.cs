using HIS.Api.Auth;
using HIS.Api.Mappers;
using HIS.Application.Services.Ratings;
using HIS.Contracts.Requests.Ratings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HIS.Api.Controllers
{
    [Authorize]
    public class RatingsController : ControllerBase
    {
        private readonly IRatingsService _ratingsService;

        public RatingsController(IRatingsService ratingsService)
        {
            _ratingsService = ratingsService;
        }

        [HttpPut(ApiEndpoints.Doctors.Rate)]
        public async Task<IActionResult> RateDoctor([FromRoute] Guid id, [FromBody] RateDoctorRequest request, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();

            var isRated = await _ratingsService.RateDoctorAsync(id, request.Rating, userId, token);

            return isRated ? Ok() : NotFound(); 
        }

        [HttpDelete(ApiEndpoints.Doctors.DeleteRating)]
        public async Task<IActionResult> DeleteRating([FromRoute] Guid id, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();

            var isDeleted = await _ratingsService.DeleteRatingAsync(id, userId, token);

            return isDeleted ? Ok() : NotFound();
        }

        [HttpGet(ApiEndpoints.Doctors.GetUserRatingsForDoctor)]
        public async Task<IActionResult> GetUserRatings(CancellationToken token = default)
        {
            var userId = HttpContext.GetUserId();

            var userRatings = await _ratingsService.GetRatingsForUserAsync(userId, token);

            var response = userRatings.MapToRatingsResponse();

            return Ok(response);
        }
    }
}
