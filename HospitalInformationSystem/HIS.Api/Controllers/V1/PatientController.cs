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
using Asp.Versioning;
using HIS.Contracts.Responses.Patients;

namespace HIS.Api.Controllers.V1
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpGet(ApiEndpoints.Patients.Get)]
        [ProducesResponseType(typeof(PatientResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ResponseCache(Duration = 30,VaryByHeader = "Accept, Accept-Encoding",Location = ResponseCacheLocation.Client)]
        public async Task<IActionResult> GetPatient([FromRoute] Guid id, CancellationToken token)
        {
            var patient = await _patientService.GetPatientAsync(id, token);

            if (patient is null)
            {
                return NotFound();
            }

            var response = patient.MapToResponse();

            return Ok(response);
        }

        [Authorize(AuthConstants.AdminPolicy)]
        [HttpGet(ApiEndpoints.Patients.GetAll)]
        [ProducesResponseType(typeof(PatientsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
        [ResponseCache(
            Duration = 30,
            VaryByQueryKeys = new[] { "FirstName", "LastName", "Surname", "DiseaseList", "Age", "SortBy" },
            VaryByHeader = "Accept, Accept-Encoding",
            Location = ResponseCacheLocation.Client
            )]
        public async Task<IActionResult> GetAllPatients([FromQuery] GetAllPatientsRequest request,
            [FromServices] LinkGenerator linkGenerator, CancellationToken token)
        {
            var options = request
                .MapToOptions();

            var patients = await _patientService.GetAllPatientsAsync(options, token);

            var patientsCount = await _patientService.GetPatientsCountAsync(options, token);

            var response = patients
                .MapToResponses(request.Page, request.PageSize, patientsCount)
                .AddLinksIntoResponse(request, linkGenerator);

            return Ok(response);
        }

        [Authorize(AuthConstants.AdminPolicy)]
        [HttpPost(ApiEndpoints.Patients.Create)]
        [ProducesResponseType(typeof(PatientResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
        [ResponseCache(
            Duration = 30,
            VaryByQueryKeys = new[] { "FirstName", "LastName", "Surname", "DiseaseList", "Age", "Email" },
            VaryByHeader = "Accept, Accept-Encoding",
            Location = ResponseCacheLocation.Client
            )]
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
        [ProducesResponseType(typeof(PatientResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
        [ResponseCache(
            Duration = 30,
            VaryByQueryKeys = new[] { "FirstName", "LastName", "Surname", "DiseaseList", "Age" },
            VaryByHeader = "Accept, Accept-Encoding",
            Location = ResponseCacheLocation.Client
            )]
        public async Task<IActionResult> UpdatePatient([FromRoute] Guid id, [FromBody] UpdatePatientRequest request, CancellationToken token)
        {
            var patient = request.MapToPatient(id);

            var result = await _patientService.UpdatePatientAsync(patient, token);

            return Ok(result);
        }

        [Authorize(AuthConstants.AdminPolicy)]
        [HttpDelete(ApiEndpoints.Patients.Delete)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ResponseCache(Duration = 30,VaryByHeader = "Accept, Accept-Encoding",Location = ResponseCacheLocation.Client)]
        public async Task<IActionResult> DeletePatient([FromRoute] Guid id, CancellationToken token)
        {
            var isDeleted = await _patientService.DeletePatientAsync(id, token);

            if (!isDeleted)
            {
                return NotFound();
            }

            return Ok(isDeleted);
        }
    }
}
