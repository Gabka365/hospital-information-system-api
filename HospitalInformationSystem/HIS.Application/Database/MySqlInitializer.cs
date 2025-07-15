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
                create table if not exists `HospitalInformationSystemDB`.`doctors` (
                Id CHAR(36) NOT NULL PRIMARY KEY,
                FirstName CHAR(100),
                LastName CHAR(100),
                Surname CHAR(100),
                Experience INT,
                Specialties CHAR(255),
                Category CHAR(100)
                )
                """);
        }
    }
}
