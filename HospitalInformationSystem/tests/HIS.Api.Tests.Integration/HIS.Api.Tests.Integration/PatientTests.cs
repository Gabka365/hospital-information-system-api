using FluentAssertions;
using HIS.Api.Endpoints.Patients;
using HIS.Api.Tests.Integration.Helpers;
using HIS.Api.Tests.Integration.States;
using HIS.Contracts.Enums;
using HIS.Contracts.Requests;
using HIS.Contracts.Requests.Doctors;
using HIS.Contracts.Requests.Patients;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HIS.Api.Tests.Integration
{
    [TestCaseOrderer("HIS.Api.Tests.Integration.Priority.AlphabeticalOrderer", "HIS.Api.Tests.Integration")]
    public class PatientTests : IClassFixture<MockAuthApiFactory>, IClassFixture<PatientTestState>
    {
        private readonly MockAuthApiFactory _mockAuthApiFactory;
        private readonly PatientTestState _patientTestState;

        public PatientTests(MockAuthApiFactory mockAuthApiFactory, PatientTestState patientTestState)
        {
            _mockAuthApiFactory = mockAuthApiFactory;
            _patientTestState = patientTestState;   
        }

        [Theory]
        [ClassData(typeof(NotExistGuids))]
        public async Task A1_Get_ReturnsNotFound_WhenPatientDoesNotExist(string id)
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
        public async Task A2_Get_ReturnsOK_WhenPatientDoesExist(string id)
        {
            // Prepare
            var client = _mockAuthApiFactory.GetAuthorizedClient();

            // Act
            var response = await client.GetAsync($"/api/patients/{id}");

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
            var response = await client.PostAsync($"/api/patients", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task A4_Create_ReturnsCreated()
        {
            //Prepare
            var client = _mockAuthApiFactory.GetAdminClient();
            var request = new CreatePatientRequest
            {
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                Surname = "TestSurname",
                Age = 30,
                DiseaseList = $"{DiseaseList.Asthma.ToString()}",
                Email = "test@mail.by"
            };
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/api/patients", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            if (response.StatusCode == HttpStatusCode.Created)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                dynamic obj = JObject.Parse(responseContent);
                _patientTestState.Id = obj.id;
            }
        }


        [Fact]
        public async Task A5_Update_ReturnsOk()
        {
            //Prepare
            var client = _mockAuthApiFactory.GetAdminClient();
            var request = new UpdatePatientRequest
            {
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                Surname = "TestSurname",
                Age = 30,
                DiseaseList = $"{DiseaseList.Hepatitis.ToString()}"
            };
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PutAsync($"/api/patients/{_patientTestState.Id}", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }


        [Theory]
        [ClassData(typeof(NotExistGuids))]
        public async Task A6_Delete_ReturnsNotFound_WhenUserDoesNotExist(Guid Id)
        {
            //Prepare
            var client = _mockAuthApiFactory.GetAdminClient();

            //Act
            var response = await client.DeleteAsync($"/api/patients/{Id}");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }


        [Fact]
        public async Task A7_Delete_ReturnsOk()
        {
            //Prepare
            var client = _mockAuthApiFactory.GetAdminClient();

            //Act
            var response = await client.DeleteAsync($"/api/patients/{_patientTestState.Id}");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task A8_GetAll_ReturnsOk()
        {
            //Prepare
            var client = _mockAuthApiFactory.GetAdminClient();

            //Act
            var response = await client.GetAsync($"/api/patients");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }


        // Доделать seed-метод
        [Fact]
        public async Task A9_AddDoctorForCurrentUser_ReturnsBadRequest_WhenRecordDoesNotPrimary()
        {
            //Prepare
            var client = _mockAuthApiFactory.GetAuthorizedClient();

            //Act
            var response = await client.GetAsync("/api/patients/add/doctor/{}");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }


        // Доделать seed-метод
        [Fact]
        public async Task A10_AddPatientForCurrentUser_ReturnsOk()
        {
            //Prepare
            var client = _mockAuthApiFactory.GetAuthorizedClient();

            //Act
            var response = await client.GetAsync("/api/patients/add/doctor/{}");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }


        // Доделать seed-метод
        [Fact]
        public async Task A11_AddDoctorForPatient_ReturnsBadRequest_WhenRecordDoesNotPrimary()
        {
            //Prepare
            var client = _mockAuthApiFactory.GetAuthorizedClient();

            //Act
            var response = await client.GetAsync("/api/patients/add/doctor/{}/patient/{}");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }


        // Доделать seed-метод
        [Fact]
        public async Task A12_AddDoctorForPatient_ReturnsOk()
        {
            //Prepare
            var client = _mockAuthApiFactory.GetAuthorizedClient();

            //Act
            var response = await client.GetAsync("/api/patients/add/doctor/{}/patient/{}");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        // Доделать seed-метод
        [Fact]
        public async Task A13_GetPatientsDoctors_ReturnsOk()
        {
            //Prepare
            var client = _mockAuthApiFactory.GetAuthorizedClient();

            //Act
            var response = await client.GetAsync("/api/patients/{}/doctors");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }


    public class NotExistGuids : IEnumerable<object[]>
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

    public class ExistGuids : IEnumerable<object[]>
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
