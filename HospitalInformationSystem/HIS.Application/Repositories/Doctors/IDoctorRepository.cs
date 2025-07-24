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
        Task<DoctorDto?> GetDoctorByIdAsync(Guid id, CancellationToken token);
        Task<bool> DeleteDoctorAsync(Guid id, CancellationToken token);
        Task<bool> CreateDoctorAsync(DoctorDto doctorDto, CancellationToken token);
        Task<bool> UpdateDoctorAsync(DoctorDto doctor, CancellationToken token);
        Task<List<DoctorDto>> GetAllDoctorsAsync(CancellationToken token);
    }
}
