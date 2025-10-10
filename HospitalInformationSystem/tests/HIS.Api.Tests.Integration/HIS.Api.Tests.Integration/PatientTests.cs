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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HIS.Api.Tests.Integration
{
    public class PatientTests : IClassFixture<MockAuthApiFactory>
    {
        private readonly MockAuthApiFactory _mockAuthApiFactory;

        public PatientTests(MockAuthApiFactory mockAuthApiFactory)
        {
            _mockAuthApiFactory = mockAuthApiFactory;
        }

        [Theory]
        [ClassData(typeof(ClassData))]
        public async Task Get_ReturnsNotFound_WhenPatientDoesNotExist(string id)
        {
            // Prepare
            var client = _mockAuthApiFactory.CreateClient();

            // Act
            var response = await client.GetAsync($"/api/patients/{id}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
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
