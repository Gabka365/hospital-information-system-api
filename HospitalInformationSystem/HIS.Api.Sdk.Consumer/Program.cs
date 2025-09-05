using HIS.Api.Sdk;
using Refit;
using System.Text.Json;

var hisApi = RestService.For<IHisApi>("https://localhost:5050");

var doctor = await hisApi.GetDoctorAsync("17ab799c-514a-485d-97d9-3363e2e62db3");

Console.WriteLine(JsonSerializer.Serialize(doctor));