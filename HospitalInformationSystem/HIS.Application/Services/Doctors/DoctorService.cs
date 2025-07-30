using FluentValidation;
using HIS.Application.Mappers;
using HIS.Application.Models;
using HIS.Application.Repositories;
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
        private readonly IValidator<Doctor> _doctorValidator;

        public DoctorService(IDoctorRepository doctorRepository, IValidator<Doctor> doctorValidator)
        {
            _doctorRepository = doctorRepository;
            _doctorValidator = doctorValidator;
        }

        public async Task<bool> CreateDoctorAsync(Doctor doctor, CancellationToken token)
        {
            _doctorValidator.ValidateAndThrow(doctor);

            var DoctorDTO = doctor.MapToDoctorDTO();

            var isCreated = await _doctorRepository.CreateDoctorAsync(DoctorDTO, token);

            return isCreated;
        }

        public async Task<bool> DeleteDoctorAsync(Guid id, CancellationToken token)
        {
            var isDeleted = await _doctorRepository.DeleteDoctorAsync(id, token);

            return isDeleted;
        }

        public async Task<List<Doctor>> GetAllDoctorsAsync(CancellationToken token)
        {
            var DoctorDTOs = await _doctorRepository.GetAllDoctorsAsync(token);

            var doctors = DoctorDTOs.Select(x => x.MapToDoctor());

            return doctors.ToList();
        }

        public async Task<Doctor?> GetDoctorByIdAsync(Guid id, CancellationToken token)
        {
            var DoctorDTO = await _doctorRepository.GetDoctorByIdAsync(id, token);

            if (DoctorDTO == null)
            {
                return null;
            }

            var doctor = DoctorDTO.MapToDoctor();

            return doctor;
        }

        public async Task<bool> UpdateDoctorAsync(Doctor doctor, CancellationToken token)
        {
            _doctorValidator.ValidateAndThrow(doctor);

            var DoctorDTO = doctor.MapToDoctorDTO();

            var isUpdated = await _doctorRepository.UpdateDoctorAsync(DoctorDTO, token);

            return isUpdated;
        }

        public async Task<List<Patient>> GetDoctorsPatientsAsync(Guid id, CancellationToken token)
        {
            var result = await _doctorRepository.GetDoctorsPatientsAsync(id, token);

            var patients = result.Select(x => x.MapToPatient()).ToList()!;

            return patients;
        }
    }
}
