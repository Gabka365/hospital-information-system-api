using HIS.Api.Mappers;
using HIS.Contracts.Requests.Patients;
using Microsoft.AspNetCore.Mvc;
using HIS.Application.Services.Patients;
using HIS.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using HIS.Api.Auth;
using HIS.Contracts.Responses;
using System.Net;
using HIS.Api;

namespace HIS.Api.Controllers.V1
{
    [Authorize]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpGet(ApiEndpoints.Patients.Get)]
        public async Task<IActionResult> GetPatient([FromRoute] Guid id, CancellationToken token)
        {
            var patient = await _patientService.GetPatientAsync(id, token);

            var response = patient.MapToResponse();

            return Ok(response);
        }

        [Authorize(AuthConstants.AdminPolicy)]
        [HttpGet(ApiEndpoints.Patients.GetAll)]
        public async Task<IActionResult> GetAllPatients([FromQuery] GetAllPatientsRequest request,
            [FromServices] LinkGenerator linkGenerator, CancellationToken token)
        {
            var options = request
                .MapToOptions();

            var patients = await _patientService.GetAllPatientsAsync(options, token);

            var patientsCount = await _patientService.GetPatientsCountAsync(options, token);

            var response = patients
                .MapToResponses(request.Page, request.PageSize, patientsCount, linkGenerator)
                .AddLinksIntoResponse(request, linkGenerator);

            return Ok(response);
        }

        [Authorize(AuthConstants.AdminPolicy)]
        [HttpPost(ApiEndpoints.Patients.Create)]
        public async Task<IActionResult> CreatePatient([FromBody] CreatePatientRequest request, CancellationToken token)
        {
            var specifiedUserId = await _patientService.GetUserIdByEmail(request.Email, token);

            var patient = request.MapToPatient(specifiedUserId);

            var result = await _patientService.CreatePatientAsync(patient, token);

            var response = result.MapToResponse();

            return Ok(response);
        }

        [Authorize(AuthConstants.AdminPolicy)]
        [HttpPut(ApiEndpoints.Patients.Update)]
        public async Task<IActionResult> UpdatePatient([FromRoute] Guid id, [FromBody] UpdatePatientRequest request, CancellationToken token)
        {
            var patient = request.MapToPatient(id);

            var result = await _patientService.UpdatePatientAsync(patient, token);

            return Ok(result);
        }

        [Authorize(AuthConstants.AdminPolicy)]
        [HttpDelete(ApiEndpoints.Patients.Delete)]
        public async Task<IActionResult> DeletePatient([FromRoute] Guid id, CancellationToken token)
        {
            var isDeleted = await _patientService.DeletePatientAsync(id, token);

            return Ok(isDeleted);
        }

        [HttpGet(ApiEndpoints.Patients.GetPatientsDoctors)]
        public async Task<IActionResult> GetPatientsDoctors([FromRoute] Guid id, CancellationToken token)
        {
            var result = await _patientService.GetPatientsDoctorsAsync(id, token);

            var response = result.MapToResponses();

            return Ok(response);
        }

        [Authorize(AuthConstants.AdminPolicy)]
        [HttpGet(ApiEndpoints.Patients.AddDoctorForPatient)]
        public async Task<IActionResult> AddDoctorForPatient([FromRoute] Guid DoctorId, [FromRoute] Guid PatientId, CancellationToken token)
        {
            var result = await _patientService.AddDoctorForPatientAsync(DoctorId, PatientId, token);

            return Ok(result);
        }

        [HttpGet(ApiEndpoints.Patients.AddDoctorForCurrentUser)]
        public async Task<IActionResult> AddDoctorForCurrentUser([FromRoute] Guid id, CancellationToken token)
        {
            var result = await _patientService.GetPatientsDoctorsAsync(id, token);

            var response = result.MapToResponses();

            return Ok(response);
        }

    }
}
