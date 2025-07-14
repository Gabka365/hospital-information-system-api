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

namespace HIS.Application.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private List<Doctor> _doctors = new List<Doctor>();

        private MySqlConnectionFactory _mySqlConnectionFactory;

        public DoctorRepository(MySqlConnectionFactory mySqlConnectionFactory) 
        { 
            _mySqlConnectionFactory = mySqlConnectionFactory;
        }

        public async Task<bool> CreateDoctorAsync(DoctorDto doctorDto)
        {
            var connection = await _mySqlConnectionFactory.CreateConnectionAsync();
            
            var count = await connection.ExecuteAsync(new CommandDefinition(
                """
                insert into `HospitalInformationSystemDB`.`doctors` 
                (id, FirstName, LastName, Surname, Category, Specialties, Experience) values (@id, 
                @FirstName, @LastName, @Surname, @Category, @Specialties, @Experience)
                """, doctorDto));

            return count > 0;
        }

        public async Task<bool> DeleteDoctorAsync(Guid id)
        {
            var connection = await _mySqlConnectionFactory.CreateConnectionAsync();

            var count = await connection.ExecuteAsync(new CommandDefinition(
                """
                delete from `HospitalInformationSystemDB`.`doctors` where id=@id
                """, new { id }));

            return count == 1;
        }

        public async Task<List<DoctorDto>> GetAllDoctorsAsync()
        {
            var connection = await _mySqlConnectionFactory.CreateConnectionAsync();

            var doctorDtos = await connection.QueryAsync<DoctorDto>(
                """
                select * from `HospitalInformationSystemDB`.`doctors`
                """);

            return doctorDtos.ToList();
        }

        public async Task<DoctorDto?> GetDoctorByIdAsync(Guid id)
        {
            var connection = await _mySqlConnectionFactory.CreateConnectionAsync();

            var doctorDto = await connection.QueryAsync<DoctorDto>(
                """
                select * from `HospitalInformationSystemDB`.`doctors` where id=@id
                """, new { id });

            return doctorDto.SingleOrDefault();  
        }

        public async Task<bool> UpdateDoctorAsync(DoctorDto doctorDto)
        {
            var connection = await _mySqlConnectionFactory.CreateConnectionAsync();

            var count = await connection.ExecuteAsync(
                """
                update `HospitalInformationSystemDB`.`doctors`
                set FirstName=@FirstName, LastName=@LastName, Surname=@Surname, Category=@Category, Specialties=@Specialties, Experience=@Experience 
                where Id = @Id
                """, doctorDto);

            return count == 1;
        }
    }
}
