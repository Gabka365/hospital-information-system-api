using HIS.Api.Mappers;
using HIS.Application.Repositories;
using HIS.Contracts.Requests;
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
        private IDoctorRepository _doctorRepository;

        public DoctorController(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        [HttpGet(ApiEndpoints.Doctors.Get)]
        public async Task<IActionResult> GetDoctor([FromRoute] Guid id)
        {
            var doctor = await _doctorRepository.GetDoctorByIdAsync(id);
            return doctor != null ? Ok(doctor.MapToResponse()) : BadRequest("Does not exist.");
        }

        [HttpGet(ApiEndpoints.Doctors.GetAll)]
        public async Task<IActionResult> GetAllDoctors()
        {
            var doctors = await _doctorRepository.GetAllDoctorsAsync();
            return Ok(doctors);
        }

        [HttpPost(ApiEndpoints.Doctors.Create)]
        public async Task<IActionResult> CreateDoctor([FromBody] CreateDoctorRequest request)
        {
            var doctor = request.MapToDoctor();
            var created = await _doctorRepository.CreateDoctorAsync(doctor);
            var response = created.MapToResponse();
            return Created($"/api/doctors/{response.Id}", response);
        }

        [HttpPut(ApiEndpoints.Doctors.Update)]
        public async Task<IActionResult> UpdateDoctor([FromRoute] Guid id, [FromBody] UpdateDoctorRequest request)
        {
            var doctor = request.MapToDoctor(id);
            var isUpdated = await _doctorRepository.UpdateDoctorAsync(doctor);

            if (!isUpdated)
            {
                return NotFound();
            }
            return Ok(doctor.MapToResponse());
        }

        [HttpDelete(ApiEndpoints.Doctors.Delete)]
        public async Task<IActionResult> DeleteDoctor([FromRoute] Guid id)
        {
            var isDeleted = await _doctorRepository.DeleteDoctorAsync(id);

            if (!isDeleted)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}
