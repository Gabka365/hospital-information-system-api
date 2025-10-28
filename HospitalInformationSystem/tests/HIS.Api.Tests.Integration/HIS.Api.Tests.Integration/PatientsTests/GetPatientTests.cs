using FluentAssertions;
using HIS.Api.Tests.Integration.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Api.Tests.Integration.PatientTests
{
    [Collection("HIS-Api Patients Collection")]
    public class GetPatientTests
    {
        private readonly MockAuthApiFactory _mockAuthApiFactory;

        public GetPatientTests(MockAuthApiFactory mockAuthApiFactory)
        {
            _mockAuthApiFactory = mockAuthApiFactory;
        }

        [Theory]
        [ClassData(typeof(NotExistGuids))]
        public async Task Get_ReturnsNotFound_WhenPatientDoesNotExist(string id)
        {
            // Prepare
            var client = _mockAuthApiFactory.CreateClient();

            // Act
            var response = await client.GetAsync($"/api/patients/{id}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Theory]
        [ClassData(typeof(ExistGuids))]
        public async Task Get_ReturnsOK_WhenPatientDoesExist(string id)
        {
            // Prepare
            var client = _mockAuthApiFactory.GetAuthorizedClient();

            // Act
            var response = await client.GetAsync($"/api/patients/{id}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }


    public class ExistGuids : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "ad0eebe6-64e3-45f0-a326-b3c040c0792e" };
            yield return new object[] { "6f88194d-a6de-41c3-9e17-890e66e248d6" };
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
