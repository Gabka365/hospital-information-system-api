using FluentValidation;
using HIS.Application.Models;
using HIS.Application.Repositories.Patients;
using HIS.Application.Validators;
using HIS.Application.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Application.Services.Patients
{
    public class PatientService : IPatientService
    {
        private readonly PatientValidator _patientValidator;
        private readonly IPatientRepository _patientRepository;

        public PatientService(PatientValidator patientValidator, IPatientRepository patientRepository)
        {
            _patientValidator = patientValidator;
            _patientRepository = patientRepository;
        }

        public async Task<Patient> GetPatientAsync(Guid id, CancellationToken token)
        {
            var patientDto = await _patientRepository.GetPatientAsync(id, token);
        
            Patient patient = patientDto.MapToPatient();    

            return patient;
        }

        public async Task<List<Patient>> GetAllPatientsAsync(CancellationToken token)
        {
            var patientDtos = await _patientRepository.GetAllPatientsAsync(token);

            var patients = patientDtos
                .Select(x => x.MapToPatient())
                .ToList();

            return patients;
        }

        public async Task<Patient> CreatePatientAsync(Patient patient, CancellationToken token)
        {
            _patientValidator.ValidateAndThrow(patient);

            var patientDto = patient.MapToPatientDto();

            var result = await _patientRepository.CreatePatientAsync(patientDto, token);

            patient = result.MapToPatient();

            return patient;
        }

        public async Task<Patient> UpdatePatientAsync(Patient patient, CancellationToken token)
        {
            _patientValidator.ValidateAndThrow(patient);

            var patientDto = patient.MapToPatientDto();

            var result = await _patientRepository.UpdatePatientAsync(patientDto, token);

            patient = result.MapToPatient();

            return patient;
        }

        public async Task<bool> DeletePatientAsync(Guid id, CancellationToken token)
        {
            var isDeleted = await _patientRepository.DeletePatientAsync(id, token);

            return isDeleted;
        }
    }
}
