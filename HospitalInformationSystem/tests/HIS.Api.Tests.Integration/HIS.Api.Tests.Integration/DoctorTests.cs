using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Api.Tests.Integration
{
    public class DoctorTests
    {
        HttpClient _client;
        WebApplicationFactory<Program> _factory;

        public DoctorTests() 
        { 
            _factory = new WebApplicationFactory<Program>();
            _client = _factory.CreateClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"Bearer {}");
        }

        [Fact]
        public async Task Get_ReturnsNotFound_WhenDoctorDoesNotExist()
        {
            var response = await _client.GetAsync($"/api/v1/doctors/{Guid.NewGuid}");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
