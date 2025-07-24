using Dapper;
using HIS.Application.Database;
using HIS.Application.DTOs;
using HIS.Application.Mappers;
using HIS.Application.Models;
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

        public async Task<bool> CreateDoctorAsync(DoctorDto doctorDto, CancellationToken token)
        {
            var connection = await _mySqlConnectionFactory.CreateConnectionAsync(token);

            var count = await connection.ExecuteAsync(new CommandDefinition(
                """
                insert into `HospitalInformationSystemDB`.`doctors` 
                (id, FirstName, LastName, Surname, Category, Specialties, Experience) values (@id, 
                @FirstName, @LastName, @Surname, @Category, @Specialties, @Experience)
                """, doctorDto, cancellationToken: token));

            return count > 0;
        }

        public async Task<bool> DeleteDoctorAsync(Guid id, CancellationToken token)
        {
            var connection = await _mySqlConnectionFactory.CreateConnectionAsync(token);

            var count = await connection.ExecuteAsync(new CommandDefinition(
                """
                delete from `HospitalInformationSystemDB`.`doctors` where id=@id
                """, new { id }, cancellationToken: token));

            return count == 1;
        }

        public async Task<List<DoctorDto>> GetAllDoctorsAsync(CancellationToken token)
        {
            var connection = await _mySqlConnectionFactory.CreateConnectionAsync(token);

            var doctorDtos = await connection.QueryAsync<DoctorDto>(new CommandDefinition(
                """
                select * from `HospitalInformationSystemDB`.`doctors`
                """, cancellationToken: token));

            return doctorDtos.ToList();
        }

        public async Task<DoctorDto?> GetDoctorByIdAsync(Guid id, CancellationToken token)
        {
            var connection = await _mySqlConnectionFactory.CreateConnectionAsync(token);

            var doctorDto = await connection.QueryAsync<DoctorDto>(new CommandDefinition(
                """
                select * from `HospitalInformationSystemDB`.`doctors` where id=@id
                """, new { id }, cancellationToken: token));

            return doctorDto.SingleOrDefault();
        }

        public async Task<bool> UpdateDoctorAsync(DoctorDto doctorDto, CancellationToken token)
        {
            var connection = await _mySqlConnectionFactory.CreateConnectionAsync(token);

            var count = await connection.ExecuteAsync(new CommandDefinition(
                """
                update `HospitalInformationSystemDB`.`doctors`
                set FirstName=@FirstName, LastName=@LastName, Surname=@Surname, Category=@Category, Specialties=@Specialties, Experience=@Experience 
                where Id = @Id
                """, doctorDto, cancellationToken: token));

            return count == 1;
        }
    }
}
