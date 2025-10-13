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
        Task<DoctorDTO?> GetDoctorByIdAsync(Guid id, Guid userId, CancellationToken token = default);
        Task<bool> DeleteDoctorAsync(Guid id, Guid userId, CancellationToken token = default);
        Task<bool> CreateDoctorAsync(DoctorDTO DoctorDTO, CancellationToken token = default);
        Task<DoctorDTO> UpdateDoctorAsync(DoctorDTO doctor, Guid userId, CancellationToken token = default);
        Task<List<DoctorDTO>> GetAllDoctorsAsync(GetAllDoctorsOptions options, CancellationToken token = default);
        Task<List<PatientDTO>> GetDoctorsPatientsAsync(Guid id, CancellationToken token = default);
        Task<bool> IsDoctorExistAsync(Guid id, CancellationToken token = default);
        Task<bool> AddPatientForDoctorAsync(Guid PatientId, Guid DoctorId, CancellationToken token = default);
        Task<int> GetDoctorsCountAsync(GetAllDoctorsOptions options, CancellationToken token = default);
    }
}
