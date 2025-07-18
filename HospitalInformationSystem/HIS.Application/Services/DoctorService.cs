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

namespace HIS.Application.Services
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

            var doctorDto = doctor.MapToDoctorDto();

            var isCreated = await _doctorRepository.CreateDoctorAsync(doctorDto, token); 
        
            return isCreated;
        }

        public async Task<bool> DeleteDoctorAsync(Guid id, CancellationToken token)
        {
            var isDeleted = await _doctorRepository.DeleteDoctorAsync(id, token);

            return isDeleted;   
        }

        public async Task<List<Doctor>> GetAllDoctorsAsync(CancellationToken token)
        {
            var doctorDtos = await _doctorRepository.GetAllDoctorsAsync(token);

            var doctors = doctorDtos.Select(x => x.MapToDoctor());

            return doctors.ToList();
        }

        public async Task<Doctor?> GetDoctorByIdAsync(Guid id, CancellationToken token)
        {
            var doctorDto = await _doctorRepository.GetDoctorByIdAsync(id, token);

            if (doctorDto == null)
            {
                return null;
            }

            var doctor = doctorDto.MapToDoctor(); 

            return doctor;
        }

        public async Task<bool> UpdateDoctorAsync(Doctor doctor, CancellationToken token)
        {
            _doctorValidator.ValidateAndThrow(doctor);

            var doctorDto = doctor.MapToDoctorDto();

            var isUpdated = await _doctorRepository.UpdateDoctorAsync(doctorDto, token);

            return isUpdated;
        }
    }
}
