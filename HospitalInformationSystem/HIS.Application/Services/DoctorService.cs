using HIS.Application.Mappers;
using HIS.Application.Models;
using HIS.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Application.Services
{
    public class DoctorService : IDoctorService
    {
        private IDoctorRepository _doctorRepository;

        public DoctorService(IDoctorRepository doctorRepository) 
        {
            _doctorRepository = doctorRepository;
        }

        public async Task<bool> CreateDoctorAsync(Doctor doctor)
        {
            var doctorDto = doctor.MapToDoctorDto();

            var isCreated = await _doctorRepository.CreateDoctorAsync(doctorDto); 
        
            return isCreated;
        }

        public async Task<bool> DeleteDoctorAsync(Guid id)
        {
            var isDeleted = await _doctorRepository.DeleteDoctorAsync(id);

            return isDeleted;   
        }

        public async Task<List<Doctor>> GetAllDoctorsAsync()
        {
            var doctorDtos = await _doctorRepository.GetAllDoctorsAsync();

            var doctors = doctorDtos.Select(x => x.MapToDoctor());

            return doctors.ToList();
        }

        public async Task<Doctor?> GetDoctorByIdAsync(Guid id)
        {
            var doctorDto = await _doctorRepository.GetDoctorByIdAsync(id);

            if (doctorDto == null)
            {
                return null;
            }

            var doctor = doctorDto.MapToDoctor(); 

            return doctor;
        }

        public async Task<bool> UpdateDoctorAsync(Doctor doctor)
        {
            var doctorDto = doctor.MapToDoctorDto();

            var isUpdated = await _doctorRepository.UpdateDoctorAsync(doctorDto);

            return isUpdated;
        }
    }
}
