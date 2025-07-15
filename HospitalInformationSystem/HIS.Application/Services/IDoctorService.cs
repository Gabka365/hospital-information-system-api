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
        Task<Doctor?> GetDoctorByIdAsync(Guid id, CancellationToken token);
        Task<bool> DeleteDoctorAsync(Guid id, CancellationToken token);
        Task<bool> CreateDoctorAsync(Doctor doctor, CancellationToken token);
        Task<bool> UpdateDoctorAsync(Doctor doctor, CancellationToken token);
        Task<List<Doctor>> GetAllDoctorsAsync(CancellationToken token);
    }
}
