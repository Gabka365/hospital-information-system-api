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
        Task<PatientDTO> GetPatientAsync(Guid id, CancellationToken token);
        Task<List<PatientDTO>?> GetAllPatientsAsync(CancellationToken token);
        Task<PatientDTO> CreatePatientAsync(PatientDTO patientDto, CancellationToken token);
        Task<PatientDTO> UpdatePatientAsync(PatientDTO patientDto, CancellationToken token);
        Task<bool> DeletePatientAsync(Guid id, CancellationToken token);
        Task<List<DoctorDTO>> GetPatientsDoctors(Guid id, CancellationToken token);
        Task<bool> IsPatientExistAsync(Guid id, CancellationToken token);
        Task<bool> AddDoctorForPatientAsync(Guid DoctorId, Guid PatientId, CancellationToken token);
    }
}
