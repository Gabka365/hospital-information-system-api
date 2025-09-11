using Asp.Versioning;
using Dapper;
using HIS.Api.Auth;
using HIS.Api.Mappers;
using HIS.Application.Database;
using HIS.Application.DTOs;
using HIS.Application.Repositories;
using HIS.Application.Repositories.Auth;
using HIS.Application.Services.Auth;
using HIS.Application.Services.Doctors;
using HIS.Application.Services.Patients;
using HIS.Contracts.Requests;
using HIS.Contracts.Requests.Doctors;
using HIS.Contracts.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Eventing.Reader;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.Intrinsics.Arm;

namespace HIS.Api.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("2.0")]
    public class DoctorController : ControllerBase
    {
        private IDoctorService _doctorService;
        
        public DoctorController(IDoctorService doctorService, IAuthRepository authRepository)
        {
            _doctorService = doctorService;
        }

        [HttpGet(ApiEndpoints.V2.Doctors.GetDoctorsPatients)]
        public async Task<ActionResult> GetDoctorPatients([FromRoute] Guid id, CancellationToken token)
        {
            var result = await _doctorService.GetDoctorsPatientsAsync(id, token);

            var response = result.MapToResponses();

            return Ok(response);
        }

        [Authorize(AuthConstants.AdminPolicy)]
        [HttpGet(ApiEndpoints.V2.Doctors.AddPatientForDoctor)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AddPatientForDoctor(Guid patientId, Guid doctorId, CancellationToken token)
        {
            var result = await _doctorService.AddPatientForDoctorAsync(patientId, doctorId, token);

            if (!result)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        [HttpGet(ApiEndpoints.V2.Doctors.AddPatientForCurrentUser)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddPatientForCurrentUser([FromRoute] Guid patientId, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();

            var result = await _doctorService.AddPatientForDoctorAsync(patientId, userId, token);

            if (!result)
            {
                return BadRequest();
            }

            return Ok(result);
        }
    }
}
