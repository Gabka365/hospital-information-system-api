using FluentAssertions;
using HIS.Api.Tests.Integration.Helpers;
using HIS.Contracts.Enums;
using HIS.Contracts.Requests.Doctors;
using HIS.Contracts.Requests.Patients;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HIS.Api.Tests.Integration.PatientTests
{
    [Collection("HIS-Api Patients Collection")]
    public class DeletePatientTests
    {
        private readonly MockAuthApiFactory _mockAuthApiFactory;
        private string _createdId;

        public DeletePatientTests(MockAuthApiFactory mockAuthApiFactory)
        {
            _mockAuthApiFactory = mockAuthApiFactory;
        }

        [Theory]
        [ClassData(typeof(NotExistGuids))]
        public async Task Delete_ReturnsNotFound_WhenUserDoesNotExist(Guid Id)
        {
            //Prepare
            var client = _mockAuthApiFactory.GetAdminClient();

            //Act
            var response = await client.DeleteAsync($"/api/patients/{Id}");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_ReturnsOk()
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
            var preresponse = await client.PostAsync("/api/patients", content);
            var responseContent = await preresponse.Content.ReadAsStringAsync();
            dynamic obj = JObject.Parse(responseContent);
            _createdId = obj.id;

            //Act
            var response = await client.DeleteAsync($"/api/patients/{_createdId}");

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

}
