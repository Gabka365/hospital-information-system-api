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
        Task<bool> CreateDoctorAsync(DoctorDTO DoctorDTO, Guid userId, CancellationToken token);
        Task<bool> UpdateDoctorAsync(DoctorDTO doctor, Guid userId, CancellationToken token);
        Task<List<DoctorDTO>> GetAllDoctorsAsync(Guid userId, CancellationToken token);
        Task<List<PatientDTO>> GetDoctorsPatientsAsync(Guid id, CancellationToken token);
    }
}
