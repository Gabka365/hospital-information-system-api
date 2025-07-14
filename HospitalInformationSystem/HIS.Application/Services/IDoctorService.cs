using HIS.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Application.Services
{
    public interface IDoctorService
    {
        Task<Doctor?> GetDoctorByIdAsync(Guid id);
        Task<bool> DeleteDoctorAsync(Guid id);
        Task<bool> CreateDoctorAsync(Doctor doctor);
        Task<bool> UpdateDoctorAsync(Doctor doctor);
        Task<List<Doctor>> GetAllDoctorsAsync();
    }
}
