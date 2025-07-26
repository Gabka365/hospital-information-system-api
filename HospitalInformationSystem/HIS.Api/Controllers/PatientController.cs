using HIS.Api.Mappers;
using HIS.Contracts.Requests.Patients;
using Microsoft.AspNetCore.Mvc;
using HIS.Application.Services.Patients;
using HIS.Application.DTOs;

namespace HIS.Api.Controllers
{
    [ApiController]
    public class PatientController : ControllerBase
    {
        private IPatientService _patientService;

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

        [HttpGet(ApiEndpoints.Patients.GetAll)]
        public async Task<IActionResult> GetAllPatients(CancellationToken token)
        {
            var patients = await _patientService.GetAllPatientsAsync(token);

            return Ok(patients);
        }

        [HttpPost(ApiEndpoints.Patients.Create)]
        public async Task<IActionResult> CreatePatient([FromBody] CreatePatientRequest request, CancellationToken token)
        {
            var patient = request.MapToPatient();
        
            var result = await _patientService.CreatePatientAsync(patient, token);

            var response = result.MapToResponse();
            
            return Ok(response);
        }

        [HttpPut(ApiEndpoints.Patients.Update)]
        public async Task<IActionResult> UpdatePatient([FromRoute] Guid id, [FromBody] UpdatePatientRequest request, CancellationToken token)
        {
            var patient = request.MapToPatient(id);
            
            var result = await _patientService.UpdatePatientAsync(patient, token);

            return Ok(result);
        }

        [HttpDelete(ApiEndpoints.Patients.Delete)]
        public async Task<IActionResult> DeletePatient([FromRoute] Guid id, CancellationToken token)
        {
            var isDeleted = await _patientService.DeletePatientAsync(id, token);    

            return Ok(isDeleted);
        }
    }
}
