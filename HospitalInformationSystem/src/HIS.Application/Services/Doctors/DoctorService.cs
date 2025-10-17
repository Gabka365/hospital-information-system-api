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
        private readonly IValidator<GetAllDoctorsOptions> _getAllDoctorsOptionsValidator;
        private readonly IAuthRepository _authRepository;

        public DoctorService(IDoctorRepository doctorRepository, IPatientRepository patientRepository, 
             IRatingsRepository ratingsRepository, IValidator<Doctor> doctorValidator, 
             IValidator<GetAllDoctorsOptions> getAllDoctorsOptionsValidator, IAuthRepository authRepository)
        {
            _doctorRepository = doctorRepository;
            _patientRepository = patientRepository;
            _ratingsRepository = ratingsRepository;
            _doctorValidator = doctorValidator;
            _getAllDoctorsOptionsValidator = getAllDoctorsOptionsValidator;
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

        public async Task<List<Doctor>> GetAllDoctorsAsync(GetAllDoctorsOptions options, CancellationToken token)
        {
            _getAllDoctorsOptionsValidator.ValidateAndThrow(options);

            var doctorDTOs = await _doctorRepository.GetAllDoctorsAsync(options, token);

            var doctors = doctorDTOs.Select(x => x.MapToDoctor());

            return doctors.ToList();
        }

        public async Task<int> GetDoctorsCountAsync(GetAllDoctorsOptions options, CancellationToken token)
        {
            _getAllDoctorsOptionsValidator.ValidateAndThrow(options);

            var doctorsCount = await _doctorRepository.GetDoctorsCountAsync(options, token);

            return doctorsCount;
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

        public async Task<Guid?> GetUserIdByEmail(string email, CancellationToken token)
        {
            var specifiedUser = await _authRepository.GetUserAsync(email, token);

            if (specifiedUser == null)
                return null;

            return specifiedUser.Id;
        }

        public async Task<bool> AddPatientForDoctorAsync(Guid patientId, Guid doctorId, CancellationToken token)
        {
            var specifiedPatient = await _patientRepository.IsPatientExistAsync(patientId, token);

            if (!specifiedPatient)
            {
                return false;
            }

            var specifiedDoctor = await _doctorRepository.IsDoctorExistAsync(doctorId, token);

            if (!specifiedDoctor)
            {
                return false;
            }

            var result = await _doctorRepository.AddPatientForDoctorAsync(patientId, doctorId, token);

            return result;
        }

        public async Task<bool> DeleteDoctorPatientAsync(Guid doctorId, Guid patientId, CancellationToken token)
        {
            var specifiedDoctor = await _doctorRepository.IsDoctorExistAsync(doctorId, token);

            if (!specifiedDoctor)
            {
                throw new InvalidDataException($"Not correct doctorID: {doctorId}");
            }

            var specifiedPatient = await _patientRepository.IsPatientExistAsync(patientId, token);

            if (!specifiedPatient)
            {
                throw new InvalidDataException($"Not correct patientID: {patientId}");
            }

            var result = await _doctorRepository.DeleteDoctorPatientAsync(doctorId, patientId, token);

            return result;
        }
    }
}
