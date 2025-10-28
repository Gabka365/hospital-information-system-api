using FluentAssertions;
using HIS.Api.Tests.Integration.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Api.Tests.Integration.PatientTests
{
    [Collection("HIS-Api Doctors Collection")]
    public class GetAllPatientTests
    {
        private readonly MockAuthApiFactory _mockAuthApiFactory;

        public GetAllPatientTests(MockAuthApiFactory mockAuthApiFactory)
        {
            _mockAuthApiFactory = mockAuthApiFactory;
        }

        [Fact]
        public async Task GetAll_ReturnsOk()
        {
            //Prepare
            var client = _mockAuthApiFactory.GetAdminClient();

            //Act
            var response = await client.GetAsync($"/api/patients");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

    }
}
