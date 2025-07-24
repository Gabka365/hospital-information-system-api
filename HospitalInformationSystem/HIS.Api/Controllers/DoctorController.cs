using HIS.Api.Mappers;
using HIS.Application.Repositories;
using HIS.Application.Services.Doctors;
using HIS.Contracts.Requests;
using HIS.Contracts.Requests.Doctors;
using HIS.Contracts.Responses;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Eventing.Reader;
using System.Runtime.Intrinsics.Arm;

namespace HIS.Api.Controllers
{
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private IDoctorService _doctorService;

        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpGet(ApiEndpoints.Doctors.Get)]
        public async Task<IActionResult> GetDoctor([FromRoute] Guid id, CancellationToken token)
        {
            var doctor = await _doctorService.GetDoctorByIdAsync(id, token);

            if (doctor is null)
            {
                return NotFound();
            }
            return Ok(doctor.MapToResponse());
        }

        [HttpGet(ApiEndpoints.Doctors.GetAll)]
        public async Task<IActionResult> GetAllDoctors(CancellationToken token)
        {
            var doctors = await _doctorService.GetAllDoctorsAsync(token);

            return Ok(doctors);
        }

        [HttpPost(ApiEndpoints.Doctors.Create)]
        public async Task<IActionResult> CreateDoctor([FromBody] CreateDoctorRequest request, CancellationToken token)
        {
            var doctor = request.MapToDoctor();
            var isCreated = await _doctorService.CreateDoctorAsync(doctor, token);
            
            if (!isCreated)
                return BadRequest();
            
            var response = doctor.MapToResponse();
            return Created($"/api/doctors/{response.Id}", response);
        }

        [HttpPut(ApiEndpoints.Doctors.Update)]
        public async Task<IActionResult> UpdateDoctor([FromRoute] Guid id, [FromBody] UpdateDoctorRequest request, CancellationToken token)
        {
            var doctor = request.MapToDoctor(id);
            var isUpdated = await _doctorService.UpdateDoctorAsync(doctor, token);

            if (!isUpdated)
            {
                return NotFound();
            }
            return Ok(doctor.MapToResponse());
        }

        [HttpDelete(ApiEndpoints.Doctors.Delete)]
        public async Task<IActionResult> DeleteDoctor([FromRoute] Guid id, CancellationToken token)
        {
            var isDeleted = await _doctorService.DeleteDoctorAsync(id, token);

            if (!isDeleted)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}
