using FluentAssertions;
using HIS.Api.Tests.Integration.Helpers;
using HIS.Contracts.Enums;
using HIS.Contracts.Requests;
using HIS.Contracts.Requests.Doctors;
using HIS.Contracts.Responses.Doctors;
using Microsoft.AspNetCore.Mvc.Testing;
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
    public class DoctorTests : IClassFixture<MockAuthApiFactory>
    {
        private readonly MockAuthApiFactory _mockAuthApiFactory;
        private Guid createdDoctorGuid = Guid.Empty;

        public DoctorTests(MockAuthApiFactory mockAuthApiFactory) 
        { 
            _mockAuthApiFactory = mockAuthApiFactory;
        }

        [Theory]
        [MemberData(nameof(NotExistGuids))]
        public async Task Get_ReturnsNotFound_WhenDoctorDoesNotExist(string id)
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
        public async Task Get_ReturnsOK_WhenDoctorDoesExist(string id)
        {
            // Prepare
            var client = _mockAuthApiFactory.GetAuthorizedClient();

            // Act
            var response = await client.GetAsync($"/api/v1/doctors/{id}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
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
            var response = await client.PostAsync($"/api/v1/doctors", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Create_ReturnsCreated()
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
                Email = "test@mail.by",
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
                var resp = JsonSerializer.Deserialize<TestResponse>(responseContent);
            }
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
            var response = await client.PutAsync($"/api/v1/doctors/{createdDoctorGuid}", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }



        public static IEnumerable<object[]> NotExistGuids { get; } = new[]
        {
            new[] { Guid.NewGuid().ToString() },
            new[] { Guid.NewGuid().ToString() },
            new[] { Guid.NewGuid().ToString() }
        };

        public static IEnumerable<object[]> ExistGuids { get; } = new[]
        {
            new[] { "0715a0b3-85e8-4d6d-b502-0aae4fb6ca37" },
            new[] { "17ab799c-514a-485d-97d9-3363e2e62db3" },
            new[] { "c0f10d02-66d2-4cc9-baef-631d6b73fcac" }
        };

    }
}
