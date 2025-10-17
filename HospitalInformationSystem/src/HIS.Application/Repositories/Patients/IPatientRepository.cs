using HIS.Application.Models;
using HIS.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Application.Repositories.Patients
{
    public interface IPatientRepository
    {
        Task<PatientDTO?> GetPatientAsync(Guid id, CancellationToken token = default);
        Task<List<PatientDTO>?> GetAllPatientsAsync(GetAllPatientsOptions options, CancellationToken token = default);
        Task<PatientDTO?> CreatePatientAsync(PatientDTO patientDto, CancellationToken token = default);
        Task<PatientDTO> UpdatePatientAsync(PatientDTO patientDto, CancellationToken token = default);
        Task<bool> DeletePatientAsync(Guid id, CancellationToken token = default);
        Task<List<DoctorDTO>> GetPatientsDoctors(Guid id, CancellationToken token = default);
        Task<bool> IsPatientExistAsync(Guid id, CancellationToken token = default);
        Task<bool> AddDoctorForPatientAsync(Guid DoctorId, Guid PatientId, CancellationToken token = default);
        Task<int> GetPatientsCountAsync(GetAllPatientsOptions options, CancellationToken token = default);
    }
}
