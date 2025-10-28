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
    public class V2DoctorTests
    {
        private readonly MockAuthApiFactory _mockAuthApiFactory;
        
        public V2DoctorTests(MockAuthApiFactory mockAuthApiFactory)
        {
            _mockAuthApiFactory = mockAuthApiFactory;
        }

        [Fact]
        public async Task AddPatientForDoctor_ReturnsBadRequest_WhenRecordDoesNotPrimary()
        {
            //Prepare
            var client = _mockAuthApiFactory.GetAdminClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json;api-version=2.0");

            //Act
            var response = await client.GetAsync("/api/v2/doctors/add/patient/ad0eebe6-64e3-45f0-a326-b3c040c0792e/" +
                "doctor/403ea9d0-b273-451a-bc50-a3494e2c0345");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }


        [Fact]
        public async Task AddPatientForDoctor_ReturnsOk()
        {
            //Prepare
            var client = _mockAuthApiFactory.GetAdminClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json;api-version=2.0");

            //Act
            var response = await client.GetAsync("/api/v2/doctors/add/patient/ad0eebe6-64e3-45f0-a326-b3c040c0792e/" +
                "doctor/adb40620-909f-4609-9781-8a353140c452");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            //Cleanup
            await client.DeleteAsync("/api/v2/doctors/delete/patient/ad0eebe6-64e3-45f0-a326-b3c040c0792e/" +
                "doctor/adb40620-909f-4609-9781-8a353140c452");
        }


        [Fact]
        public async Task GetDoctorsPatients_ReturnsOk()
        {
            //Prepare
            var client = _mockAuthApiFactory.GetAuthorizedClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json;api-version=2.0");

            //Act
            var response = await client.GetAsync("/api/v2/doctors/403ea9d0-b273-451a-bc50-a3494e2c0345/patients");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

    }
}
