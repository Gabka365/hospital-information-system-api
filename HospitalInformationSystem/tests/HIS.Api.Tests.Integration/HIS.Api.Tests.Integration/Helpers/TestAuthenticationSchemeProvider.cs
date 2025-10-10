using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Api.Tests.Integration.Helpers
{
    public class TestAuthenticationSchemeProvider : AuthenticationSchemeProvider
    {
        public const string Name = "TestAuthenticationScheme";

        public TestAuthenticationSchemeProvider(IOptions<AuthenticationOptions> options) : base(options)
        {
        }

        protected TestAuthenticationSchemeProvider(IOptions<AuthenticationOptions> options, IDictionary<string, AuthenticationScheme> schemes) : base(options, schemes)
        {
        }

        public override Task<AuthenticationScheme?> GetDefaultAuthenticateSchemeAsync()
        {
            var scheme = new AuthenticationScheme(Name, Name, typeof(TestAuthenticationHandler));
            return Task.FromResult(scheme)!;
        }
    }
}
