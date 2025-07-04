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
        Task<Doctor?> GetDoctorByIdAsync(Guid id);
        Task<bool> DeleteDoctorAsync(Guid id);
        Task<Doctor> CreateDoctorAsync(Doctor doctor);
        Task<bool> UpdateDoctorAsync(Doctor doctor);
        Task<List<Doctor>> GetAllDoctorsAsync();
    }
}
