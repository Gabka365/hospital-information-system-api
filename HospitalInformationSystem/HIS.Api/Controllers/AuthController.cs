using HIS.Api.Mappers;
using HIS.Application.Services.Auth;
using HIS.Contracts.Requests.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HIS.Api.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost(ApiEndpoints.Auth.Register)]
        public async Task<IActionResult> Register([FromBody] UserRequest request, CancellationToken token)
        {
            var user = request.MapToRegisterUser();

            var loggedUser = await _authService.RegisterUserAsync(user, token);

            var response = user.MapToResponse();

            return Ok(response);
        }

        [HttpPost(ApiEndpoints.Auth.Login)]
        public async Task<IActionResult> Login([FromBody] UserRequest request, CancellationToken token)
        {
            var customClaims = request.CustomClaims;
            var user = request.MapToLoggedUser();

            var loggedUser = await _authService.LoginUserAsync(user, customClaims, token);

            if (loggedUser == null)
            {
                return BadRequest("Try another one.");
            }

            var response = loggedUser!.MapToResponse();

            return Ok(response);
        }
    }
}
