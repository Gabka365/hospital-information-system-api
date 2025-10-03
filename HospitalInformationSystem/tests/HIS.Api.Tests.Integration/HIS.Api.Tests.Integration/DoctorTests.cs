using FluentAssertions;
using HIS.Api.Tests.Integration.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Api.Tests.Integration
{
    public class DoctorTests : IClassFixture<WebApplicationFactory<IApiMarker>>, IAsyncLifetime, IDisposable
    {
        private readonly HttpClient _httpClient;

        public DoctorTests(WebApplicationFactory<IApiMarker> appFactory) 
        { 
            _httpClient = appFactory.CreateClient();
        }

        [Theory]
        [MemberData(nameof(Data))]
        //[ClassData(typeof(ClassData))]
        public async Task Get_ReturnsNotFound_WhenDoctorDoesNotExist(string id)
        {
            _httpClient.DefaultRequestHeaders.Authorization
                    = new AuthenticationHeaderValue("Bearer", AuthHelper.GenerateToken());

            // Act
            var response =  await _httpClient.GetAsync($"/api/v1/doctors/{id}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        public Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {   
        
        }

        public static IEnumerable<object[]> Data { get; } = new[]
        {
            new[] { Guid.NewGuid().ToString() },
            new[] { Guid.NewGuid().ToString() },
            new[] { Guid.NewGuid().ToString() }
        };

    }


    public class ClassData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { Guid.NewGuid().ToString() };
            yield return new object[] { Guid.NewGuid().ToString() };
            yield return new object[] { Guid.NewGuid().ToString() };
        }

        IEnumerator<object[]> IEnumerable<object[]>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
