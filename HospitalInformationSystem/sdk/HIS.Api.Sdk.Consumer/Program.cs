using HIS.Api.Sdk;
using Refit;
using System.Security.Authentication.ExtendedProtection;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using HIS.Api.Sdk.Consumer;
using HIS.Contracts.Requests.Doctors;
using HIS.Contracts.Enums;
using HIS.Contracts.Requests;
using HIS.Contracts.Requests.Patients;

var services = new ServiceCollection();

services
    .AddHttpClient()
    .AddSingleton<AuthTokenProvider>()
    .AddRefitClient<IHisApi>(x => new RefitSettings
    {
        AuthorizationHeaderValueGetter = async (request, CancellationToken) => await x.GetRequiredService<AuthTokenProvider>().GetTokenAsync(),
    })
    .ConfigureHttpClient(x =>
    {
        x.BaseAddress = new Uri("https://localhost:5050");
        x.DefaultRequestHeaders.Add("x-api-key", "123");
    });
var provider = services.BuildServiceProvider();

var hisApi = provider.GetRequiredService<IHisApi>();

#region Doctors SDK

//Get doctor by id
var doctor = await hisApi.GetDoctorAsync("17ab799c-514a-485d-97d9-3363e2e62db3");


//Get first 2 doctors
var doctors = await hisApi.GetDoctorsAsync(new GetAllDoctorsRequest
{
    //filter parameters
    FirstName = null,
    LastName = null,
    Surname = null,
    SortBy = null,
    Specialties = null,
    Category = null,
    Experience = null,
    Page = 1,
    PageSize = 2,
});


//Create specified doctor
var createdDoctor = await hisApi.CreateDoctorAsync(new CreateDoctorRequest
{
    FirstName = "Bob",
    LastName = "Qwerty",
    Surname = "Timberlake",
    Specialties = new List<Speciality> { Speciality.Gynecologist, Speciality.Pediatrician },
    Category = Category.Second,
    Experience = 20,
    Email = "doctor9@mail.by"
});


//Update the doctor that was created previously
var updatedDoctor = await hisApi.UpdateDoctorAsync(createdDoctor.Id.ToString(), new UpdateDoctorRequest
{
    FirstName = createdDoctor.FirstName,
    LastName = createdDoctor.LastName,
    Surname = createdDoctor.Surname,
    Category = createdDoctor.Category,
    Experience = createdDoctor.Experience,
    Specialties = new List<Speciality>() { Speciality.Radiologist, Speciality.Dermatologist, Speciality.Cardiologist }
});

//Delete the doctor that was created previously
var isDoctorDeleted = await hisApi.DeleteDoctorAsync(createdDoctor.Id.ToString());

////Get all patients that have doctor with this id
//var patientsWithSpecifiedDoctor = await hisApi.GetDoctorsPatientsAsync("65a7fde3-36ab-441a-8399-3dde790bb475");

////Add patient to the specified doctor
//var isPatientAddedToDoctor = await hisApi.AddPatientForDoctorAsync("d13e717c-26ab-41c1-90be-e986c47f23e1", "17ab799c-514a-485d-97d9-3363e2e62db3");

////Add patient to the current user
//var isPatientAddedToCurrentUser = await hisApi.AddPatientForCurrentUserAsync("d13e717c-26ab-41c1-90be-e986c47f23e1");

#endregion


#region Patients SDK

var patient = await hisApi.GetPaientAsync("8c86adf6-9cd4-484a-93b8-7be5ead786fc");

//Get first 2 patients
var patients = await hisApi.GetPatientsAsync(new GetAllPatientsRequest
{
    //filter parameters
    FirstName = null,
    LastName = null,
    Surname = null,
    SortBy = null,
    Age = null,
    DiseaseList = null,
    Page = 1,
    PageSize = 2,
});


//Create specified patient
var createdPatient = await hisApi.CreatePatientAsync(new CreatePatientRequest
{
    FirstName = "Bob",
    LastName = "Qwerty",
    Surname = "Timberlake",
    DiseaseList = "Cancer",
    Age = 20,
    Email = "patient9@mail.by"
});


//Update the patient that was created previously
var updatedPatient = await hisApi.UpdatePatientAsync(createdPatient.Id.ToString(), new UpdatePatientRequest
{
    FirstName = "Bob",
    LastName = "Qwerty",
    Surname = "Timberlake",
    DiseaseList = "Cancer",
    Age = 40
});

//Delete the patient that was created previously
var isPatientDeleted = await hisApi.DeletePatientAsync(createdPatient.Id.ToString());

////Get all doctors that have patient with this id
//var doctorsWithSpecifiedPatient = await hisApi.GetPatientsDoctorsAsync("13401029-9ed4-4d19-bf9b-1c87b3e78416");

////Add doctor to the specified patient
//var isDoctorAddedToPatient = await hisApi.AddDoctorForPatientAsync("65a7fde3-36ab-441a-8399-3dde790bb475", "d13e717c-26ab-41c1-90be-e986c47f23e1");

////Add doctor to the current user
//var isDoctorAddedToCurrentUser = await hisApi.AddDoctorForCurrentUserAsync("65a7fde3-36ab-441a-8399-3dde790bb475");

#endregion
