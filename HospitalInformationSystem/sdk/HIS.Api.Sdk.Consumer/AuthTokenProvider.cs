using HIS.Contracts.Requests.Auth;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Api.Sdk.Consumer
{
    public class AuthTokenProvider
    {
        private readonly HttpClient _httpClient;
        private string _cachedToken = string.Empty; 
        private static readonly SemaphoreSlim Lock = new (1,1);

        public AuthTokenProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetTokenAsync()
        {
            if (!string.IsNullOrEmpty(_cachedToken))
            {
                var jwt = new JwtSecurityTokenHandler().ReadJwtToken(_cachedToken);
                var expiryTimeText = jwt.Claims.Single(claim => claim.Type == "exp").Value;
                var expiryDateTime = UnixTimeStampToDateTime(int.Parse(expiryTimeText));
                if (expiryDateTime > DateTime.UtcNow)
                {
                    return _cachedToken;
                }
            }

            await Lock.WaitAsync();

            var response = await _httpClient.PostAsJsonAsync("https://localhost:5050/api/auth/login", new UserRequest
            {
                UserName = "admin",
                Password = "root",
                Email = "admin@mail.by",
                CustomClaims = new Dictionary<string, object> 
                { 
                    { "admin", true }, 
                    { "trusted_member", true } 
                }
            });

            IEnumerable<string> cookies = response.Headers.SingleOrDefault(header => header.Key == "Set-Cookie").Value;

            var newToken = cookies.First().Substring(4, cookies.First().Length - 12);
            _cachedToken = newToken;
            Lock.Release();

            return newToken;
        }

        private DateTime UnixTimeStampToDateTime(int time)
        {
            var datetime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            datetime = datetime.AddSeconds(time).ToLocalTime();
            return datetime;
        }
    }
}
