using HIS.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Application.Repositories.Patients
{
    public class PatientRepository : IPatientRepository
    {
        public async Task<Patient> GetPatientAsync(Guid id, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Patient>> GetAllPatientsAsync(CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public async Task<Patient> CreatePatientAsync(Patient patient, CancellationToken token)
        {

            throw new NotImplementedException();
        }

        public async Task<Patient> UpdatePatientAsync(Patient patient, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeletePatientAsync(Guid id, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
