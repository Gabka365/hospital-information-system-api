using HIS.Api.Auth;
using HIS.Application.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace HIS.Api.Tests.Integration.Helpers
{
    public class TestAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public TestAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, 
            ILoggerFactory logger, UrlEncoder encoder) : base(options, logger, encoder)
        {
        }

        public TestAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, 
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claims = new List<Claim>();

            if(Context.Request.Headers.TryGetValue("role", out var roles))
            {
                claims.Add(new Claim(ClaimTypes.Role, roles[0]!));
            }

            claims.Add(new("UserId", Guid.NewGuid().ToString()));
            claims.Add(new("UserName", "Test"));
            claims.Add(new("Email", "test@mail.by"));
            
            var identity = new ClaimsIdentity(claims, TestAuthenticationSchemeProvider.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, TestAuthenticationSchemeProvider.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }

    }
}
