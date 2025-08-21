using Dapper;
using HIS.Api.Auth;
using HIS.Api.Mappers;
using HIS.Application.Database;
using HIS.Application.DTOs;
using HIS.Application.Repositories;
using HIS.Application.Repositories.Auth;
using HIS.Application.Services.Auth;
using HIS.Application.Services.Doctors;
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
    public class DoctorController : ControllerBase
    {
        private IDoctorService _doctorService;
        
        public DoctorController(IDoctorService doctorService, IAuthRepository authRepository)
        {
            _doctorService = doctorService;
        }

        [HttpGet(ApiEndpoints.Doctors.Get)]
        public async Task<IActionResult> GetDoctor([FromRoute] Guid id, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            var doctor = await _doctorService.GetDoctorByIdAsync(id, userId, token);

            if (doctor is null)
            {
                return NotFound();
            }
            return Ok(doctor.MapToResponse());
        }

        [Authorize(AuthConstants.TrustedMemberPolicy)]
        [HttpGet(ApiEndpoints.Doctors.GetAll)]
        public async Task<IActionResult> GetAllDoctors(CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            var doctors = await _doctorService.GetAllDoctorsAsync(userId, token);

            return Ok(doctors);
        }

        [Authorize(AuthConstants.AdminPolicy)]
        [HttpPost(ApiEndpoints.Doctors.Create)]
        public async Task<IActionResult> CreateDoctor([FromBody] CreateDoctorRequest request, CancellationToken token)
        {
            var specifiedUserId = await _doctorService.GetUserIdByEmail(request.Email, token);

            var doctor = request.MapToDoctor(specifiedUserId);

            var isCreated = await _doctorService.CreateDoctorAsync(doctor, token);
            
            if (!isCreated)
                return BadRequest();
            
            var response = doctor.MapToResponse();
            return Created($"/api/doctors/{response.Id}", response);
        }

        [Authorize(AuthConstants.AdminPolicy)]
        [HttpPut(ApiEndpoints.Doctors.Update)]
        public async Task<IActionResult> UpdateDoctor([FromRoute] Guid id, [FromBody] UpdateDoctorRequest request, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();

            var doctor = request.MapToDoctor(id);
            var updatedDoctor = await _doctorService.UpdateDoctorAsync(doctor, userId, token);
            var response = updatedDoctor.MapToResponse();   
            
            return Ok(response);
        }

        [Authorize(AuthConstants.AdminPolicy)]
        [HttpDelete(ApiEndpoints.Doctors.Delete)]
        public async Task<IActionResult> DeleteDoctor([FromRoute] Guid id, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            var isDeleted = await _doctorService.DeleteDoctorAsync(id, userId, token);

            if (!isDeleted)
            {
                return NotFound();
            }
            return Ok();
        }


        [HttpGet(ApiEndpoints.Doctors.GetDoctorsPatients)]
        public async Task<ActionResult> GetDoctorsPatients(Guid id, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            var result = await _doctorService.GetDoctorsPatientsAsync(id, token);

            var response = result.MapToResponses();

            return Ok(response);
        }

        [Authorize(AuthConstants.AdminPolicy)]
        [HttpGet(ApiEndpoints.Doctors.AddPatientForDoctor)]
        public async Task<ActionResult> AddPatientForDoctor(Guid PatientId, Guid DoctorId, CancellationToken token)
        {
            var result = await _doctorService.AddPatientForDoctorAsync(PatientId, DoctorId, token);

            return Ok(result);
        }
    }
}
