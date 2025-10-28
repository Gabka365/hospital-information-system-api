using FluentAssertions;
using HIS.Api.Tests.Integration.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit.Extensions.Ordering;

namespace HIS.Api.Tests.Integration.DoctorTests
{
    [Collection("HIS-Api Doctors Collection")]
    public class GetDoctorTests
    {
        private readonly MockAuthApiFactory _mockAuthApiFactory;
        
        public GetDoctorTests(MockAuthApiFactory mockAuthApiFactory)
        {
            _mockAuthApiFactory = mockAuthApiFactory;
        }

        [Theory]
        [MemberData(nameof(NotExistGuids))]
        public async Task Get_ReturnsNotFound_WhenDoctorDoesNotExist(string id)
        {
            // Prepare
            var client = _mockAuthApiFactory.GetAuthorizedClient();

            // Act
            var response = await client.GetAsync($"/api/v1/doctors/{id}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Theory]
        [MemberData(nameof(ExistGuids))]
        public async Task Get_ReturnsOK_WhenDoctorDoesExist(string id)
        {
            // Prepare
            var client = _mockAuthApiFactory.GetAuthorizedClient();

            // Act
            var response = await client.GetAsync($"/api/v1/doctors/{id}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        public static IEnumerable<object[]> NotExistGuids { get; } = new[]
{
            new[] { Guid.NewGuid().ToString() },
            new[] { Guid.NewGuid().ToString() },
            new[] { Guid.NewGuid().ToString() }
        };

        public static IEnumerable<object[]> ExistGuids { get; } = new[]
        {
            new[] { "403ea9d0-b273-451a-bc50-a3494e2c0345" },
            new[] { "adb40620-909f-4609-9781-8a353140c452" }
        };
    }
}
