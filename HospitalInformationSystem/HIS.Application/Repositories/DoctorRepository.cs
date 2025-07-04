using HIS.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Application.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private List<Doctor> _doctors { get; set; } = new List<Doctor>();

        public Task<Doctor> CreateDoctorAsync(Doctor doctor)
        {
            _doctors.Add(doctor);

            return Task.FromResult(doctor);
        }

        public Task<bool> DeleteDoctorAsync(Guid id)
        {
            var removed = _doctors.RemoveAll(x => x?.Id == id);

            return removed != null ? Task.FromResult(true) : Task.FromResult(false);
        }

        public Task<List<Doctor>> GetAllDoctorsAsync()
        {
            return Task.FromResult(_doctors);
        }

        public Task<Doctor?> GetDoctorByIdAsync(Guid id)
        {
            var doctor = _doctors.FirstOrDefault(x => x.Id == id);

            return Task.FromResult(doctor);
        }

        public Task<bool> UpdateDoctorAsync(Doctor doctor)
        {
            var updatedIndex = _doctors.FindIndex(x => x.Id == doctor.Id);

            if (updatedIndex == -1)
            {
                return Task.FromResult(false);
            }

            _doctors[updatedIndex] = doctor;
            return Task.FromResult(true);
        }
    }
}
