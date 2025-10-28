using FluentAssertions;
using HIS.Api.Tests.Integration.Helpers;
using HIS.Contracts.Enums;
using HIS.Contracts.Requests.Doctors;
using HIS.Contracts.Requests.Patients;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HIS.Api.Tests.Integration.PatientTests
{
    [Collection("HIS-Api Patients Collection")]
    public class CreatePatientTests : IAsyncLifetime
    {
        private readonly MockAuthApiFactory _mockAuthApiFactory;
        private string _createdId;

        public CreatePatientTests(MockAuthApiFactory mockAuthApiFactory)
        {
            _mockAuthApiFactory = mockAuthApiFactory;
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_WhenNotCorrectEmail()
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
        public async Task Create_ReturnsCreated()
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
                Email = "testadmin@mail.by"
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
                _createdId = obj.id;
            }
        }

        public async Task DisposeAsync()
        {
            var client = _mockAuthApiFactory.GetAdminClient();

            await client.DeleteAsync($"/api/patients/{_createdId}");
        }

        public Task InitializeAsync() => Task.CompletedTask;    
    }
}
