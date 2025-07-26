using Dapper;
using HIS.Application.Database;
using HIS.Application.Models;
using HIS.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Application.Repositories.Patients
{
    public class PatientRepository : IPatientRepository
    {
        private MySqlConnectionFactory _mySqlConnectionFactory;

        public PatientRepository(MySqlConnectionFactory mySqlConnectionFactory) 
        {
            _mySqlConnectionFactory = mySqlConnectionFactory;
        }

        public async Task<PatientDTO> GetPatientAsync(Guid id, CancellationToken token)
        {
            var connection = await _mySqlConnectionFactory.CreateConnectionAsync(token);

            var result = await connection.QuerySingleOrDefaultAsync<PatientDTO>(new CommandDefinition("""
                select * from `HospitalInformationSystemDB`.`patients`
                where Id=@id
                """, new { id }, cancellationToken: token));
        
            if (result == null)
            {
                throw new Exception($"Patients table hasn't been uploaded with this record.");
            }

            return result;
        }

        public async Task<List<PatientDTO>?> GetAllPatientsAsync(CancellationToken token)
        {
            var connection = await _mySqlConnectionFactory.CreateConnectionAsync(token);

            var result = await connection.QueryAsync<PatientDTO>(new CommandDefinition("""
                select * from `HospitalInformationSystemDB`.`patients`
                """, cancellationToken: token));

            if (result == null)
            {
                throw new Exception($"Patients table hasn't been uploaded with this record.");
            }

            return result.ToList();
        }

        public async Task<PatientDTO> CreatePatientAsync(PatientDTO patientDto, CancellationToken token)
        {
            var connection = await _mySqlConnectionFactory.CreateConnectionAsync(token);
            
            var result = await connection.ExecuteAsync(new CommandDefinition("""
                insert into `HospitalInformationSystemDB`.`patients` (Id, FirstName, LastName, Surname, Age, DiseaseList)
                values (@Id, @FirstName, @LastName, @Surname, @Age, @DiseaseList)
                """, patientDto, cancellationToken: token));

            if (result != 1)
            {
                throw new InvalidDataException("Your data is invalid.");
            }

            return patientDto;
        }

        public async Task<PatientDTO> UpdatePatientAsync(PatientDTO patientDto, CancellationToken token)
        {
            var connection = await _mySqlConnectionFactory.CreateConnectionAsync(token);

            var result = await connection.ExecuteAsync(new CommandDefinition("""
                update `HospitalInformationSystemDB`.`patients`
                set FirstName=@FirstName, LastName=@LastName, Surname=@Surname, Age=@Age, DiseaseList=@DiseaseList
                where Id=@Id
                """, patientDto, cancellationToken: token));

            if (result != 1)
            {
                throw new InvalidDataException("Your data is invalid.");
            }

            return patientDto;
        }

        public async Task<bool> DeletePatientAsync(Guid id, CancellationToken token)
        {
            var connection = await _mySqlConnectionFactory.CreateConnectionAsync(token);

            var result = await connection.ExecuteAsync(new CommandDefinition("""
                delete from `HospitalInformationSystemDB`.`patients` where Id=@Id
                """, new { id }, cancellationToken: token));

            return result == 1 ? true : false;  
        }
    }
}
