using FluentValidation;
using HIS.Application.Mappers;
using HIS.Application.Models;
using HIS.Application.Repositories;
using HIS.Application.Repositories.Auth;
using HIS.Application.Repositories.Patients;
using HIS.Application.Repositories.Ratings;
using HIS.Application.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Application.Services.Doctors
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IRatingsRepository _ratingsRepository;
        private readonly IValidator<Doctor> _doctorValidator;
        private readonly IAuthRepository _authRepository;

        public DoctorService(IDoctorRepository doctorRepository, IPatientRepository patientRepository, 
            IRatingsRepository ratingsRepository, IValidator<Doctor> doctorValidator, IAuthRepository authRepository)
        {
            _doctorRepository = doctorRepository;
            _patientRepository = patientRepository;
            _ratingsRepository = ratingsRepository;
            _doctorValidator = doctorValidator;
            _authRepository = authRepository;
        }

        public async Task<bool> CreateDoctorAsync(Doctor doctor, CancellationToken token)
        {
            _doctorValidator.ValidateAndThrow(doctor);

            var DoctorDTO = doctor.MapToDoctorDTO();

            var isCreated = await _doctorRepository.CreateDoctorAsync(DoctorDTO, token);

            return isCreated;
        }

        public async Task<bool> DeleteDoctorAsync(Guid id, Guid userId, CancellationToken token)
        {
            var isDeleted = await _doctorRepository.DeleteDoctorAsync(id, userId, token);

            return isDeleted;
        }

        public async Task<List<Doctor>> GetAllDoctorsAsync(Guid userId, CancellationToken token)
        {
            var DoctorDTOs = await _doctorRepository.GetAllDoctorsAsync(userId, token);

            var doctors = DoctorDTOs.Select(x => x.MapToDoctor());

            return doctors.ToList();
        }

        public async Task<Doctor?> GetDoctorByIdAsync(Guid id, Guid userId, CancellationToken token)
        {
            var DoctorDTO = await _doctorRepository.GetDoctorByIdAsync(id, userId, token);

            if (DoctorDTO == null)
            {
                return null;
            }

            var doctor = DoctorDTO.MapToDoctor();

            return doctor;
        }

        public async Task<Doctor> UpdateDoctorAsync(Doctor doctor, Guid userId, CancellationToken token)
        {
            _doctorValidator.ValidateAndThrow(doctor);

            var doctorDTO = doctor.MapToDoctorDTO();
            var updatedDoctorDto = await _doctorRepository.UpdateDoctorAsync(doctorDTO, userId, token);
            var ratings = await _ratingsRepository.GetRatingAsync(doctor.Id, userId, token);

            var updatedDoctor = updatedDoctorDto.MapToDoctor();
            updatedDoctor.Rating = ratings.Rating;
            updatedDoctor.UserRating = ratings.UserRating;

            return updatedDoctor;
        }

        public async Task<List<Patient>> GetDoctorsPatientsAsync(Guid id, CancellationToken token)
        {
            var result = await _doctorRepository.GetDoctorsPatientsAsync(id, token);

            var patients = result.Select(x => x.MapToPatient()).ToList()!;

            return patients;
        }

        public async Task<Guid> GetUserIdByEmail(string email, CancellationToken token)
        {
            var specifiedUser = await _authRepository.GetUserAsync(email, token);

            if (specifiedUser == null)
                throw new InvalidDataException($"No one concurrence with this email: {email}");

            return specifiedUser.Id;
        }

        public async Task<bool> AddPatientForDoctorAsync(Guid PatientId, Guid DoctorId, CancellationToken token)
        {
            var specifiedPatient = await _patientRepository.IsPatientExistAsync(PatientId, token);

            if (!specifiedPatient)
            {
                throw new InvalidDataException($"Not correct patientID: {PatientId}");
            }

            var specifiedDoctor = await _doctorRepository.IsDoctorExistAsync(DoctorId, token);

            if (!specifiedDoctor)
            {
                throw new InvalidDataException($"Not correct doctorID: {DoctorId}");
            }

            var result = await _doctorRepository.AddPatientForDoctorAsync(PatientId, DoctorId, token);

            return result;
        }
    }
}
