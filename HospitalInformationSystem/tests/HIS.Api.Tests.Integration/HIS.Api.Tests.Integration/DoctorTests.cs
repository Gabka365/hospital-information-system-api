using FluentAssertions;
using HIS.Api.Tests.Integration.Helpers;
using HIS.Api.Tests.Integration.States;
using HIS.Contracts.Enums;
using HIS.Contracts.Requests;
using HIS.Contracts.Requests.Doctors;
using HIS.Contracts.Responses.Doctors;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HIS.Api.Tests.Integration
{
    [TestCaseOrderer("HIS.Api.Tests.Integration.Priority.AlphabeticalOrderer", "HIS.Api.Tests.Integration")]
    public class DoctorTests : IClassFixture<MockAuthApiFactory>, IClassFixture<DoctorTestState>
    {
        private readonly MockAuthApiFactory _mockAuthApiFactory;
        private readonly DoctorTestState _doctorTestState;

        public DoctorTests(MockAuthApiFactory mockAuthApiFactory, DoctorTestState doctorTestState) 
        { 
            _mockAuthApiFactory = mockAuthApiFactory;
            _doctorTestState = doctorTestState;
        }

        [Theory]
        [MemberData(nameof(NotExistGuids))]
        public async Task A1_Get_ReturnsNotFound_WhenDoctorDoesNotExist(string id)
        {
            // Prepare
            var client = _mockAuthApiFactory.GetAuthorizedClient();

            // Act
            var response =  await client.GetAsync($"/api/v1/doctors/{id}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Theory]
        [MemberData(nameof(ExistGuids))]
        public async Task A2_Get_ReturnsOK_WhenDoctorDoesExist(string id)
        {
            // Prepare
            var client = _mockAuthApiFactory.GetAuthorizedClient();

            // Act
            var response = await client.GetAsync($"/api/v1/doctors/{id}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }


        [Fact]
        public async Task A3_Create_ReturnsBadRequest_WhenNotCorrectEmail()
        {
            //Prepare
            var client = _mockAuthApiFactory.GetAdminClient();
            var request = new CreateDoctorRequest
            {
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                Surname = "TestSurname",
                Specialties = new List<Speciality> { Speciality.Cardiologist },
                Category = Category.First,
                Experience = 10,
                Email = "TestEmail",
            };
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync($"/api/v1/doctors", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task A4_Create_ReturnsCreated()
        {
            //Prepare
            var client = _mockAuthApiFactory.GetAdminClient();
            var request = new CreateDoctorRequest
            {
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                Surname = "TestSurname",
                Specialties = new List<Speciality> { Speciality.Cardiologist },
                Category = Category.First,
                Experience = 10,
                Email = "testadmin@mail.by",
            };
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/api/v1/doctors", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            if (response.StatusCode == HttpStatusCode.Created)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                dynamic obj = JObject.Parse(responseContent);
                _doctorTestState.Id = obj.id;
            }
        }


        [Fact]
        public async Task A5_Update_ReturnsOk()
        {
            //Prepare
            var client = _mockAuthApiFactory.GetAdminClient();
            var request = new UpdateDoctorRequest
            {
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                Surname = "TestSurname",
                Specialties = new List<Speciality> { Speciality.Cardiologist },
                Category = Category.First,
                Experience = 15
            };
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PutAsync($"/api/v1/doctors/{_doctorTestState.Id}", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }


        [Theory]
        [MemberData(nameof(NotExistGuids))]
        public async Task A6_Delete_ReturnsNotFound_WhenUserDoesNotExist(Guid Id)
        {
            //Prepare
            var client = _mockAuthApiFactory.GetAdminClient();

            //Act
            var response = await client.DeleteAsync($"/api/v1/doctors/{Id}");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }


        [Fact]
        public async Task A7_Delete_ReturnsOk()
        {
            //Prepare
            var client = _mockAuthApiFactory.GetAdminClient();

            //Act
            var response = await client.DeleteAsync($"/api/v1/doctors/{_doctorTestState.Id}");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task A8_GetAll_ReturnsOk()
        {
            //Prepare
            var client = _mockAuthApiFactory.GetAdminClient();

            //Act
            var response = await client.GetAsync($"/api/v1/doctors");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task A9_AddPatientForCurrentUser_ReturnsBadRequest_WhenRecordDoesNotPrimary()
        {
            //Prepare
            var client = _mockAuthApiFactory.GetAuthorizedClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json;api-version=2.0");
            client.DefaultRequestHeaders.Add("IsDoctor", "true");

            //Act
            var response = await client.GetAsync("/api/v2/doctors/add/patient/ad0eebe6-64e3-45f0-a326-b3c040c0792e");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest); 
        }


        [Fact]
        public async Task A10_AddPatientForCurrentUser_ReturnsOk()
        {
            //Prepare
            var client = _mockAuthApiFactory.GetAdminClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json;api-version=2.0");
            client.DefaultRequestHeaders.Add("IsDoctor", "true");

            //Act
            var response = await client.GetAsync("/api/v2/doctors/add/patient/6f88194d-a6de-41c3-9e17-890e66e248d6");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            //Cleanup
            await client.DeleteAsync("/api/patients/delete/doctor/403ea9d0-b273-451a-bc50-a3494e2c0345/" +
                "patient/6f88194d-a6de-41c3-9e17-890e66e248d6");
        }


        [Fact]
        public async Task A11_AddPatientForDoctor_ReturnsBadRequest_WhenRecordDoesNotPrimary()
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
        public async Task A12_AddPatientForDoctor_ReturnsOk()
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
        public async Task A13_GetDoctorsPatients_ReturnsOk()
        {
            //Prepare
            var client = _mockAuthApiFactory.GetAuthorizedClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json;api-version=2.0");

            //Act
            var response = await client.GetAsync("/api/v2/doctors/403ea9d0-b273-451a-bc50-a3494e2c0345/patients");

            //Assert
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
