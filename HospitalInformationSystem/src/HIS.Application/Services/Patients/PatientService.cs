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
using HIS.Application.Repositories.Auth;
using HIS.Application.Repositories;

namespace HIS.Application.Services.Patients
{
    public class PatientService : IPatientService
    {
        private readonly PatientValidator _patientValidator;
        private readonly IValidator<GetAllPatientsOptions> _getAllPatientsOptionsValidator;
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IAuthRepository _authRepository;

        public PatientService(PatientValidator patientValidator, IValidator<GetAllPatientsOptions> getAllPatientsOptionsValidator, 
            IPatientRepository patientRepository, IDoctorRepository doctorRepository, IAuthRepository authRepository)
        {
            _patientValidator = patientValidator;
            _getAllPatientsOptionsValidator = getAllPatientsOptionsValidator;
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
            _authRepository = authRepository;
        }

        public async Task<Patient?> GetPatientAsync(Guid id, CancellationToken token)
        {
            var patientDto = await _patientRepository.GetPatientAsync(id, token);
        
            Patient? patient = patientDto?.MapToPatient();    

            return patient;
        }

        public async Task<List<Patient>> GetAllPatientsAsync(GetAllPatientsOptions options, CancellationToken token)
        {
            _getAllPatientsOptionsValidator.ValidateAndThrow(options);   

            var patientDtos = await _patientRepository.GetAllPatientsAsync(options, token);

            var patients = patientDtos
                .Select(x => x.MapToPatient())
                .ToList();

            return patients;
        }

        public async Task<int> GetPatientsCountAsync(GetAllPatientsOptions options, CancellationToken token)
        {
            _getAllPatientsOptionsValidator.ValidateAndThrow(options);

            var count = await _patientRepository.GetPatientsCountAsync(options, token);

            return count;
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

        public async Task<List<Doctor>> GetPatientsDoctorsAsync(Guid id, CancellationToken token)
        {
            var doctorsDtos = await _patientRepository.GetPatientsDoctors(id, token);

            var doctors = doctorsDtos.Select(x => x.MapToDoctor());

            return doctors.ToList();
        }

        public async Task<Guid> GetUserIdByEmail(string email, CancellationToken token)
        {
            var specifiedUser = await _authRepository.GetUserAsync(email, token);

            if (specifiedUser == null)
                throw new InvalidDataException($"No one concurrence with this email: {email}");

            return specifiedUser.Id;
        }

        public async Task<bool> AddDoctorForPatientAsync(Guid DoctorId, Guid PatientId, CancellationToken token)
        {
            var specifiedDoctor = await _doctorRepository.IsDoctorExistAsync(DoctorId, token);

            if (!specifiedDoctor)
            {
                throw new InvalidDataException($"Not correct doctorID: {DoctorId}");
            }

            var specifiedPatient = await _patientRepository.IsPatientExistAsync(PatientId, token);

            if (!specifiedPatient)
            {
                throw new InvalidDataException($"Not correct patientID: {PatientId}");
            }

            var result = await _patientRepository.AddDoctorForPatientAsync(DoctorId, PatientId, token);

            return result;
        }
    }
}
