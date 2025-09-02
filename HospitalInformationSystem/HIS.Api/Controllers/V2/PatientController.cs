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

namespace HIS.Api.Controllers.V2
{
    [Authorize]
    [ApiController]
    [ApiVersion("2.0")]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpGet(ApiEndpoints.Patients.GetPatientsDoctors)]
        [ProducesResponseType(typeof(PatientsResponseWithoutPagination), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPatientsDoctors([FromRoute] Guid id, CancellationToken token)
        {
            var result = await _patientService.GetPatientsDoctorsAsync(id, token);

            var response = result.MapToResponses();

            return Ok(response);
        }

        [Authorize(AuthConstants.AdminPolicy)]
        [HttpGet(ApiEndpoints.Patients.AddDoctorForPatient)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddDoctorForPatient([FromRoute] Guid DoctorId, [FromRoute] Guid PatientId, CancellationToken token)
        {
            var result = await _patientService.AddDoctorForPatientAsync(DoctorId, PatientId, token);

            return Ok(result);
        }

        [HttpGet(ApiEndpoints.Patients.AddDoctorForCurrentUser)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddDoctorForCurrentUser([FromRoute] Guid doctorId, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();

            var result = await _patientService.AddDoctorForPatientAsync(doctorId, userId, token);

            if (!result)
            {
                return BadRequest();
            }

            return Ok(result);
        }
    }
}
