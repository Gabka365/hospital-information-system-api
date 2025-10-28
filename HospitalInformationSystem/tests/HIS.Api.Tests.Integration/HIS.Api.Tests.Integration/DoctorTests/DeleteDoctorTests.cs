using FluentAssertions;
using HIS.Api.Tests.Integration.Helpers;
using HIS.Contracts.Enums;
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
    public class DeleteDoctorTests
    {
        private readonly MockAuthApiFactory _mockAuthApiFactory;
        private string _createdId;

        public DeleteDoctorTests(MockAuthApiFactory mockAuthApiFactory)
        {
            _mockAuthApiFactory = mockAuthApiFactory;
        }

        [Theory]
        [MemberData(nameof(NotExistGuids))]
        public async Task Delete_ReturnsNotFound_WhenUserDoesNotExist(Guid Id)
        {
            //Prepare
            var client = _mockAuthApiFactory.GetAdminClient();

            //Act
            var response = await client.DeleteAsync($"/api/v1/doctors/{Id}");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }


        [Fact]
        public async Task Delete_ReturnsOk()
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
                Email = "testadmin@mail.by"
            };
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var preresponse = await client.PostAsync($"/api/v1/doctors", content);
            var responseContent = await preresponse.Content.ReadAsStringAsync();
            dynamic obj = JObject.Parse(responseContent);
            _createdId = obj.id;

            //Act
            var response = await client.DeleteAsync($"/api/v1/doctors/{_createdId}");

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
