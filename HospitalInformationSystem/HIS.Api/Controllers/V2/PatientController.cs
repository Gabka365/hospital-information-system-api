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
