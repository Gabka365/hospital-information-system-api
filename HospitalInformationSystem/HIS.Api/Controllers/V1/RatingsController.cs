using Asp.Versioning;
using HIS.Api.Auth;
using HIS.Api.Mappers;
using HIS.Application.Models;
using HIS.Application.Services.Ratings;
using HIS.Contracts.Requests.Ratings;
using HIS.Contracts.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HIS.Api.Controllers.V1
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    public class RatingsController : ControllerBase
    {
        private readonly IRatingsService _ratingsService;

        public RatingsController(IRatingsService ratingsService)
        {
            _ratingsService = ratingsService;
        }

        [HttpPut(ApiEndpoints.V1.Doctors.Rate)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RateDoctor([FromRoute] Guid id, [FromBody] RateDoctorRequest request, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();

            var isRated = await _ratingsService.RateDoctorAsync(id, request.Rating, userId, token);

            return isRated ? Ok() : NotFound();
        }

        [HttpDelete(ApiEndpoints.V1.Doctors.DeleteRating)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteRating([FromRoute] Guid id, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();

            var isDeleted = await _ratingsService.DeleteRatingAsync(id, userId, token);

            return isDeleted ? Ok() : NotFound();
        }

        [HttpGet(ApiEndpoints.V1.Doctors.GetUserRatingsForDoctor)]
        [ProducesResponseType(typeof(IEnumerable<DoctorRating>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserRatings(CancellationToken token = default)
        {
            var userId = HttpContext.GetUserId();

            var userRatings = await _ratingsService.GetRatingsForUserAsync(userId, token);

            var response = userRatings.MapToRatingsResponse();

            return Ok(response);
        }
    }
}
