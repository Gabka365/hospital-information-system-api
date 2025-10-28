using FluentAssertions;
using HIS.Api.Tests.Integration.Helpers;
using HIS.Contracts.Enums;
using HIS.Contracts.Requests;
using HIS.Contracts.Requests.Doctors;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit.Extensions.Ordering;

namespace HIS.Api.Tests.Integration.DoctorTests
{
    [Collection("HIS-Api Doctors Collection")]
    public class UpdateDoctorTests: IAsyncLifetime
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
            var response = await client.PutAsync($"/api/v1/doctors/{_createdId}", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }


        public async Task InitializeAsync()
        {
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
            var response = await client.PostAsync($"/api/v1/doctors", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            dynamic obj = JObject.Parse(responseContent);
            _createdId = obj.id;
        }

        public async Task DisposeAsync()
        {
            var client = _mockAuthApiFactory.GetAdminClient();

            await client.DeleteAsync($"/api/v1/doctors/{_createdId}");
        }
    }
}
