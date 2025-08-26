using HIS.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Application.Services.Doctors
{
    public interface IDoctorService
    {
        Task<Doctor?> GetDoctorByIdAsync(Guid id, Guid userId, CancellationToken token);
        Task<bool> DeleteDoctorAsync(Guid id, Guid userId, CancellationToken token);
        Task<bool> CreateDoctorAsync(Doctor doctor, CancellationToken token);
        Task<Doctor> UpdateDoctorAsync(Doctor doctor, Guid userId, CancellationToken token);
        Task<List<Doctor>> GetAllDoctorsAsync(GetAllDoctorsOptions options, CancellationToken token);
        Task<List<Patient>> GetDoctorsPatientsAsync(Guid id, CancellationToken token);
        Task<Guid> GetUserIdByEmail(string email, CancellationToken token);
        Task<bool> AddPatientForDoctorAsync(Guid PatientId, Guid DoctorId, CancellationToken token);
        Task<int> GetDoctorsCountAsync(GetAllDoctorsOptions options, CancellationToken token);
    }
}
