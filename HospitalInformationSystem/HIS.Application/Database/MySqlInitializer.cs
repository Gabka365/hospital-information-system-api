using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Application.Database
{
    public class MySqlInitializer
    {
        private MySqlConnectionFactory _mySqlConnectionFactory;

        public MySqlInitializer(MySqlConnectionFactory mySqlConnectionFactory) 
        { 
            _mySqlConnectionFactory = mySqlConnectionFactory;
        }

        public async Task InitializeAsync(CancellationToken token = default)
        {
            var connection = await _mySqlConnectionFactory.CreateConnectionAsync(token);

            await connection.ExecuteAsync("""
                create database if not exists `HospitalInformationSystemDB`
                """);

            await connection.ExecuteAsync("""
                create table if not exists `HospitalInformationSystemDB`.`users` (
                Id CHAR(36) not null primary key,
                UserName CHAR(100) not null,
                HashedPassword CHAR(100) not null,
                Email CHAR(100) not null unique
                )
                """);

            await connection.ExecuteAsync("""
                create table if not exists `HospitalInformationSystemDB`.`doctors` (
                Id CHAR(36) not null primary key,
                FirstName CHAR(100),
                LastName CHAR(100),
                Surname CHAR(100),
                Experience INT,
                Specialties CHAR(255),
                Category CHAR(100),
                foreign key (Id) references `HospitalInformationSystemDB`.`users`(Id)
                )
                """);

            await connection.ExecuteAsync("""
                create table if not exists `HospitalInformationSystemDB`.`patients` (
                Id CHAR(36) not null primary key,
                FirstName CHAR(100),
                LastName CHAR(100),
                Surname CHAR(100),
                Age INT,
                DiseaseList CHAR(100),
                foreign key (Id) references `HospitalInformationSystemDB`.`users`(Id)
                )
                """);

            await connection.ExecuteAsync("""
                create table if not exists `HospitalInformationSystemDB`.`patientsdoctors` (
                PatientId CHAR(36) not null,
                DoctorId CHAR(36) not null,
                primary key (PatientId, DoctorId),
                foreign key (PatientId) references `HospitalInformationSystemDB`.`patients`(Id),
                foreign key (DoctorId) references `HospitalInformationSystemDB`.`doctors`(Id)
                )
                """);

        }
    }
}
