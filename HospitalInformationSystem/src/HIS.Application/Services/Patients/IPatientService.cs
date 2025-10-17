using HIS.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Application.Services.Patients
{
    public interface IPatientService
    {
        Task<Patient?> GetPatientAsync(Guid id, CancellationToken token);
        Task<List<Patient>> GetAllPatientsAsync(GetAllPatientsOptions options, CancellationToken token);
        Task<Patient> CreatePatientAsync(Patient patient, CancellationToken token);
        Task<Patient> UpdatePatientAsync(Patient patient, CancellationToken token);
        Task<bool> DeletePatientAsync(Guid id, CancellationToken token);
        Task<List<Doctor>> GetPatientsDoctorsAsync(Guid id, CancellationToken token);
        Task<Guid> GetUserIdByEmail(string email, CancellationToken token);
        Task<bool> AddDoctorForPatientAsync(Guid DoctorId, Guid PatientId, CancellationToken token);
        Task<int> GetPatientsCountAsync(GetAllPatientsOptions options, CancellationToken token);
        Task<bool> DeletePatientDoctorAsync(Guid DoctorId, Guid PatientId, CancellationToken token);
    }
}
