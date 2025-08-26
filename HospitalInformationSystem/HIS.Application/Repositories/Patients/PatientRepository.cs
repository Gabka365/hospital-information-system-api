using Dapper;
using HIS.Application.Database;
using HIS.Application.Models;
using HIS.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using HIS.Contracts.Enums;

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

        public async Task<List<PatientDTO>?> GetAllPatientsAsync(GetAllPatientsOptions options, CancellationToken token)
        {
            var connection = await _mySqlConnectionFactory.CreateConnectionAsync(token);

            var orderClause = string.Empty;
            if (options.SortField is not null)
            {
                orderClause = $"order by p.{options.SortField} {(options.SortOrder == SortOrder.Ascending ? "asc" : "desc")}";
            }

            options.FirstName = "%" + options.FirstName + "%";
            options.LastName = "%" + options.LastName + "%";
            options.Surname = "%" + options.Surname + "%";
            options.DiseaseList = "%" + options.DiseaseList + "%";

            var result = await connection.QueryAsync<PatientDTO>(new CommandDefinition($"""
                select * from `HospitalInformationSystemDB`.`patients` p
                where (@Age is null or p.Age = @Age) 
                and (@FirstName is null or p.FirstName like @FirstName) 
                and (@LastName is null or p.LastName like @LastName) 
                and (@Surname is null or p.Surname like @Surname) 
                and (@DiseaseList is null or p.DiseaseList like @DiseaseList) 
                {orderClause}
                """, options, cancellationToken: token));

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

        public async Task<List<DoctorDTO>> GetPatientsDoctors(Guid id, CancellationToken token)
        {
            var connection = await _mySqlConnectionFactory.CreateConnectionAsync(token);

            var result = await connection.QueryAsync<DoctorDTO>(new CommandDefinition("""
                select d.*
                from `HospitalInformationSystemDB`.`patients` p
                inner join `HospitalInformationSystemDB`.`patientsdoctors` pd on pd.PatientId = p.Id
                inner join `HospitalInformationSystemDB`.`doctors` d on pd.DoctorId = d.Id  
                where p.Id = @id
                """, new { id }, cancellationToken: token));

            return result.ToList();
        }

        public async Task<bool> IsPatientExistAsync(Guid id, CancellationToken token)
        {
            var connection = await _mySqlConnectionFactory.CreateConnectionAsync(token);

            var result = await connection.QueryAsync<DoctorDTO>(new CommandDefinition("""
                select *
                from `HospitalInformationSystemDB`.`patients` p
                where p.Id = @id
                """, new { id }, cancellationToken: token));

            return result.Count() == 1;
        }

        public async Task<bool> AddDoctorForPatientAsync(Guid DoctorId, Guid PatientId, CancellationToken token)
        {
            var connection = await _mySqlConnectionFactory.CreateConnectionAsync(token);

            var result = await connection.ExecuteAsync(new CommandDefinition("""
                insert into `HospitalInformationSystemDB`.`patientsdoctors` (DoctorId, PatientId)
                values (@DoctorId, @PatientId)
                """, new { DoctorId, PatientId }, cancellationToken: token));

            return result == 1;
        }
    }
}
