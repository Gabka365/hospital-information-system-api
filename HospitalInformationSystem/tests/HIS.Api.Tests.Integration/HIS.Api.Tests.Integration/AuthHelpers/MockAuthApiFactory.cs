
using HIS.Api.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Api.Tests.Integration.Helpers
{
    public class MockAuthApiFactory : WebApplicationFactory<IApiMarker>
    {
        public HttpClient HttpClient { get; private set; }

        public MockAuthApiFactory()
        {
            HttpClient = CreateClient();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureLogging(logging =>
            {
                logging.ClearProviders();
            });

            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll<IHostedService>();

                services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = TestAuthenticationSchemeProvider.Name;
                    x.DefaultChallengeScheme = TestAuthenticationSchemeProvider.Name;
                }).AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>(
                    TestAuthenticationSchemeProvider.Name,
                    _ => { });
            });
        }

        public HttpClient GetAuthorizedClient()
        {
            var client = CreateClient();

            client.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue(TestAuthenticationSchemeProvider.Name);

            return client;
        }


        public HttpClient GetAdminClient()
        {
            var client = CreateClient();

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(TestAuthenticationSchemeProvider.Name);

            client.DefaultRequestHeaders.Add(AuthConstants.ApiKeyHeaderName, "123");
            client.DefaultRequestHeaders.Add(AuthConstants.TrustedClaimType, "true");

            return client;
        }

    }
}
