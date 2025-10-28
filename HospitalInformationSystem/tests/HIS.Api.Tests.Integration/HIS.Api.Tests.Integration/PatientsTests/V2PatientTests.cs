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
    [Collection("HIS-Api Patients Collection")]
    public class V2PatientTests
    {
        private readonly MockAuthApiFactory _mockAuthApiFactory;

        public V2PatientTests(MockAuthApiFactory mockAuthApiFactory)
        {
            _mockAuthApiFactory = mockAuthApiFactory;
        }

        [Fact]
        public async Task AddDoctorForCurrentUser_ReturnsBadRequest_WhenRecordDoesNotPrimary()
        {
            //Prepare
            var client = _mockAuthApiFactory.GetAuthorizedClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json;api-version=2.0");
            client.DefaultRequestHeaders.Add("IsPatient", "true");

            //Act
            var response = await client.GetAsync("/api/patients/add/doctor/403ea9d0-b273-451a-bc50-a3494e2c0345");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }


        [Fact]
        public async Task AddDoctorForCurrentUser_ReturnsOk()
        {
            //Prepare
            var client = _mockAuthApiFactory.GetAdminClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json;api-version=2.0");
            client.DefaultRequestHeaders.Add("IsPatient", "true");

            //Act
            var response = await client.GetAsync("/api/patients/add/doctor/adb40620-909f-4609-9781-8a353140c452");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            //Cleanup
            await client.DeleteAsync("/api/patients/delete/doctor/adb40620-909f-4609-9781-8a353140c452/" +
                "patient/ad0eebe6-64e3-45f0-a326-b3c040c0792e");
        }


        [Fact]
        public async Task AddDoctorForPatient_ReturnsBadRequest_WhenRecordDoesNotPrimary()
        {
            //Prepare
            var client = _mockAuthApiFactory.GetAdminClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json;api-version=2.0");

            //Act
            var response = await client.GetAsync("/api/patients/add/doctor/adb40620-909f-4609-9781-8a353140c452/" +
                "patient/6f88194d-a6de-41c3-9e17-890e66e248d6");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }


        [Fact]
        public async Task AddDoctorForPatient_ReturnsOk()
        {
            //Prepare
            var client = _mockAuthApiFactory.GetAdminClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json;api-version=2.0");

            //Act
            var response = await client.GetAsync("/api/patients/add/doctor/adb40620-909f-4609-9781-8a353140c452/" +
                "patient/ad0eebe6-64e3-45f0-a326-b3c040c0792e");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            //Cleanup
            await client.DeleteAsync("/api/patients/delete/doctor/adb40620-909f-4609-9781-8a353140c452/" +
                "patient/ad0eebe6-64e3-45f0-a326-b3c040c0792e");
        }

        [Fact]
        public async Task GetPatientsDoctors_ReturnsOk()
        {
            //Prepare
            var client = _mockAuthApiFactory.GetAuthorizedClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json;api-version=2.0");

            //Act
            var response = await client.GetAsync("/api/patients/ad0eebe6-64e3-45f0-a326-b3c040c0792e/doctors");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
