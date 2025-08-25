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
        Task<DoctorDTO?> GetDoctorByIdAsync(Guid id, Guid userId, CancellationToken token);
        Task<bool> DeleteDoctorAsync(Guid id, Guid userId, CancellationToken token);
        Task<bool> CreateDoctorAsync(DoctorDTO DoctorDTO, CancellationToken token);
        Task<DoctorDTO> UpdateDoctorAsync(DoctorDTO doctor, Guid userId, CancellationToken token);
        Task<List<DoctorDTO>> GetAllDoctorsAsync(GetAllDoctorsOptions options, CancellationToken token);
        Task<List<PatientDTO>> GetDoctorsPatientsAsync(Guid id, CancellationToken token);
        Task<bool> IsDoctorExistAsync(Guid id, CancellationToken token);
        Task<bool> AddPatientForDoctorAsync(Guid PatientId, Guid DoctorId, CancellationToken token);
    }
}
