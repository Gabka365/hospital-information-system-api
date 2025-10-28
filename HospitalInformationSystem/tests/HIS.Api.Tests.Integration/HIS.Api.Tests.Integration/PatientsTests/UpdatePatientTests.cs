using FluentAssertions;
using HIS.Api.Tests.Integration.Helpers;
using HIS.Contracts.Enums;
using HIS.Contracts.Requests;
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
    public class UpdateDoctorTests : IAsyncLifetime
    {
        private readonly MockAuthApiFactory _mockAuthApiFactory;
        private string _createdId;

        public UpdateDoctorTests(MockAuthApiFactory mockAuthApiFactory)
        {
            _mockAuthApiFactory = mockAuthApiFactory;
        }

        [Fact]
        public async Task Update_ReturnsOk()
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
            var response = await client.PutAsync($"/api/patients/{_createdId}", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }


        public async Task InitializeAsync()
        {
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
            var response = await client.PostAsync("/api/patients", content);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var responseContent = await response.Content.ReadAsStringAsync();
            dynamic obj = JObject.Parse(responseContent);
            _createdId = obj.id;
        }

        public async Task DisposeAsync()
        {
            var client = _mockAuthApiFactory.GetAdminClient();

            await client.DeleteAsync($"/api/patients{_createdId}");
        }
    }
}
