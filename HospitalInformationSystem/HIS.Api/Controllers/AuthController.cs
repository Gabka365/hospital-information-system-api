using HIS.Api.Mappers;
using HIS.Application.Services.Auth;
using HIS.Contracts.Requests.Auth;
using HIS.Contracts.Requests.Tokens;
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
        private const string TokenSecret = "ForTheLoveOfGodStoreAndLoadThisSecurely";
        private static readonly TimeSpan TokenLifetime = TimeSpan.FromHours(8);
        private IAuthService _authService;

        public AuthController(IAuthService authService) 
        { 
            _authService = authService;
        }

        [HttpPost("token")]
        public IActionResult Index([FromBody]TokenGenerationRequest request)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(TokenSecret);

            var claims = new List<Claim>
            { 
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new("userid", request.UserId.ToString()),
                new(JwtRegisteredClaimNames.Sub, request.Email),
                new(JwtRegisteredClaimNames.Email, request.Email),
            };

            foreach (var claimPair in request.CustomClaims)
            {
                var jsonElement = (JsonElement)claimPair.Value;
                var valueType = jsonElement.ValueKind switch
                {
                    JsonValueKind.True => ClaimValueTypes.Boolean,
                    JsonValueKind.False => ClaimValueTypes.Boolean, 
                    JsonValueKind.Number => ClaimValueTypes.Double,
                    _ => ClaimValueTypes.String,
                };

                var claim = new Claim(claimPair.Key, claimPair.Value.ToString()!, valueType);
                claims.Add(claim);
            }

            var tokenDecriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(TokenLifetime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDecriptor);

            var jwt = tokenHandler.WriteToken(token);

            return Ok(jwt);
        }


        [HttpPost(ApiEndpoints.Auth.Register)]
        public async Task<IActionResult> Register([FromBody] UserRequest request, CancellationToken token)
        {
            var user = request.MapToUser();

            var loggedUser = await _authService.RegisterUserAsync(user, token);

            return Ok(loggedUser);
        }

        [HttpPost(ApiEndpoints.Auth.Login)]
        public IActionResult Login([FromBody] UserRequest request, CancellationToken token)
        {
            var user = request.MapToUser();

            return Ok();
        }
    }
}
