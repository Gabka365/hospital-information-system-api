using FluentAssertions;
using HIS.Api.Tests.Integration.Helpers;
using HIS.Contracts.Enums;
using HIS.Contracts.Requests;
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
    public class GetAllDoctorTests 
    {
        private readonly MockAuthApiFactory _mockAuthApiFactory;
        
        public GetAllDoctorTests(MockAuthApiFactory mockAuthApiFactory)
        {
            _mockAuthApiFactory = mockAuthApiFactory;
        }

        [Fact]
        public async Task GetAll_ReturnsOk()
        {
            //Prepare
            var client = _mockAuthApiFactory.GetAdminClient();

            //Act
            var response = await client.GetAsync($"/api/v1/doctors");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
