using HIS.Application.DTOs;
using HIS.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Application.Repositories
{
    public interface IDoctorRepository
    {
        Task<DoctorDto?> GetDoctorByIdAsync(Guid id);
        Task<bool> DeleteDoctorAsync(Guid id);
        Task<bool> CreateDoctorAsync(DoctorDto doctorDto);
        Task<bool> UpdateDoctorAsync(DoctorDto doctor);
        Task<List<DoctorDto>> GetAllDoctorsAsync();
    }
}
