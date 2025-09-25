using Dapper;
using HIS.Application.Database;
using HIS.Application.DTOs;
using HIS.Application.Mappers;
using HIS.Application.Models;
using HIS.Contracts.Enums;
using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Application.Repositories.Doctors
{
    public class DoctorRepository : IDoctorRepository
    {
        private MySqlConnectionFactory _mySqlConnectionFactory;

        public DoctorRepository(MySqlConnectionFactory mySqlConnectionFactory)
        {
            _mySqlConnectionFactory = mySqlConnectionFactory;
        }

        public async Task<bool> CreateDoctorAsync(DoctorDTO DoctorDTO, CancellationToken token)
        {
            var connection = await _mySqlConnectionFactory.CreateConnectionAsync(token);

            
            var count = await connection.ExecuteAsync(new CommandDefinition(
                """
                insert into `HospitalInformationSystemDB`.`doctors` 
                (id, FirstName, LastName, Surname, Category, Specialties, Experience) values (@id, 
                @FirstName, @LastName, @Surname, @Category, @Specialties, @Experience)
                """, DoctorDTO, cancellationToken: token));

            return count > 0;
        }

        public async Task<bool> DeleteDoctorAsync(Guid id, Guid userId, CancellationToken token)
        {
            var connection = await _mySqlConnectionFactory.CreateConnectionAsync(token);

            var isConnectedWithPatient = GetDoctorByIdAsync(id, userId, token) == null ? false : true;

            if (!isConnectedWithPatient)
            {
                return false;
            }

            await connection.ExecuteAsync(new CommandDefinition(
            """
                delete from `HospitalInformationSystemDB`.`patientsdoctors` where DoctorId=@id
                """, new { id }, cancellationToken: token));

            var count = await connection.ExecuteAsync(new CommandDefinition(
                """
                delete from `HospitalInformationSystemDB`.`doctors` where id=@id
                """, new { id }, cancellationToken: token));

            return count == 1;
        }

        public async Task<List<DoctorDTO>> GetAllDoctorsAsync(GetAllDoctorsOptions options, CancellationToken token)
        {
            var connection = await _mySqlConnectionFactory.CreateConnectionAsync(token);

            var orderClause = string.Empty;
            if (options.SortField is not null)
            {
                orderClause = $", d.{options.SortField} order by d.{options.SortField} {(options.SortOrder == SortOrder.Ascending ? "asc" : "desc")}";
            }

            var result = await connection.QueryAsync<DoctorDTO>(new CommandDefinition($"""
                select d.*, round(avg(r.Rating), 1) as Rating, myr.Rating as UserRating
                from `HospitalInformationSystemDB`.`doctors` d 
                left join `HospitalInformationSystemDB`.`ratings` r on d.Id = r.DoctorId
                left join `HospitalInformationSystemDB`.`ratings` myr on d.Id = myr.DoctorId and myr.UserId = @UserId 
                where (@FirstName is null or d.FirstName like @FirstName)
                and (@LastName is null or d.LastName like @LastName)
                and (@Surname is null or d.Surname like @Surname)
                and (@Specialties is null or d.Specialties like @Specialties)
                and (@Category is null or d.Category like @Category)
                and (@Experience is null or d.Experience=@Experience)
                group by d.Id{orderClause}
                limit @PageSize
                offset @PageOffset
                """, options, cancellationToken: token));

            return result.ToList();
        }

        public async Task<int> GetDoctorsCountAsync(GetAllDoctorsOptions options, CancellationToken token)
        {
            var connection = await _mySqlConnectionFactory.CreateConnectionAsync(token);

            var count = await connection.QuerySingleAsync<int>(new CommandDefinition($"""
                select count(*)
                from `HospitalInformationSystemDB`.`doctors` d 
                where (@FirstName is null or d.FirstName like @FirstName)
                and (@LastName is null or d.LastName like @LastName)
                and (@Surname is null or d.Surname like @Surname)
                and (@Specialties is null or d.Specialties like @Specialties)
                and (@Category is null or d.Category like @Category)
                and (@Experience is null or d.Experience=@Experience)
                """, options, cancellationToken: token));

            return count;
        }

        public async Task<DoctorDTO?> GetDoctorByIdAsync(Guid id, Guid userId, CancellationToken token)
        {
            var connection = await _mySqlConnectionFactory.CreateConnectionAsync(token);

            var DoctorDTO = await connection.QueryAsync<DoctorDTO>(new CommandDefinition(
                """
                select d.*, round(avg(r.Rating), 1) as Rating, myr.Rating as UserRating
                from `HospitalInformationSystemDB`.`doctors` d 
                left join `HospitalInformationSystemDB`.`ratings` r on d.Id = r.DoctorId
                left join `HospitalInformationSystemDB`.`ratings` myr on d.Id = myr.DoctorId and myr.UserId = @userId 
                where d.Id = @Id
                group by d.Id
                """, new { id, userId }, cancellationToken: token));

            return DoctorDTO.SingleOrDefault();
        }

        public async Task<DoctorDTO> UpdateDoctorAsync(DoctorDTO DoctorDTO, Guid userId, CancellationToken token)
        {
            var connection = await _mySqlConnectionFactory.CreateConnectionAsync(token);

            var count = await connection.ExecuteAsync(new CommandDefinition(
                """
                update `HospitalInformationSystemDB`.`doctors`
                set FirstName=@FirstName, LastName=@LastName, Surname=@Surname, Category=@Category, Specialties=@Specialties, Experience=@Experience 
                where Id = @Id
                """, DoctorDTO, cancellationToken: token));

            var doctorId = DoctorDTO.Id;

            var result = await connection.QueryAsync<DoctorDTO>(new CommandDefinition(
                """
                select * 
                from `HospitalInformationSystemDB`.`doctors`
                where Id = @doctorId
                """, new { doctorId }, cancellationToken: token));

            return result.Single();
        }

        public async Task<List<PatientDTO>> GetDoctorsPatientsAsync(Guid id, CancellationToken token)
        {
            var connection = await _mySqlConnectionFactory.CreateConnectionAsync(token);

            var result = await connection.QueryAsync<PatientDTO>(new CommandDefinition("""
                select p.*
                from `HospitalInformationSystemDB`.`doctors` d
                inner join `HospitalInformationSystemDB`.`patientsdoctors` pd on pd.DoctorId = d.Id
                inner join `HospitalInformationSystemDB`.`patients` p on pd.PatientId = p.Id  
                where d.Id = @id
                """, new { id }, cancellationToken: token));

            return result.ToList();
        }


        public async Task<bool> IsDoctorExistAsync(Guid id, CancellationToken token)
        {
            var connection = await _mySqlConnectionFactory.CreateConnectionAsync(token);

            var result = await connection.QueryAsync<PatientDTO>(new CommandDefinition("""
                select *
                from `HospitalInformationSystemDB`.`doctors` d
                where d.Id = @id
                """, new { id }, cancellationToken: token));

            return result.Count() == 1;
        }

        public async Task<bool> AddPatientForDoctorAsync(Guid PatientId, Guid DoctorId, CancellationToken token)
        {
            var connection = await _mySqlConnectionFactory.CreateConnectionAsync(token);

            var result = await connection.ExecuteAsync(new CommandDefinition("""
                insert into `HospitalInformationSystemDB`.`patientsdoctors` (PatientId, DoctorId)
                values (@PatientId, @DoctorId)
                """, new { PatientId, DoctorId }, cancellationToken: token));

            return result == 1;

        }
    }
}
