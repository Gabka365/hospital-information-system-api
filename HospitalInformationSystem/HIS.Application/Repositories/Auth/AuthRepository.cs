using Dapper;
using HIS.Application.Database;
using HIS.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Application.Repositories.Auth
{
    public class AuthRepository : IAuthRepository
    {
        MySqlConnectionFactory _connector;

        public AuthRepository(MySqlConnectionFactory connector) 
        { 
            _connector = connector;
        }

        public async Task<UserDTO> CreateUserAsync(UserDTO userDto, CancellationToken token)
        {
            var connection = await _connector.CreateConnectionAsync(token);

            var result = await connection.ExecuteAsync(new CommandDefinition("""
                insert into `HospitalInformationSystemDB`.`users` 
                (Id, UserName, HashedPassword, Email) values 
                (@Id, @UserName, @HashedPassword, @Email)
                """, userDto, cancellationToken: token));

            if (result != 1)
            {
                throw new Exception("Error with executing mysql query");
            }

            return userDto;
        }

        public async Task<UserDTO?> GetUserAsync(string email, CancellationToken token)
        {
            var connection = await _connector.CreateConnectionAsync(token);

            var result = await connection.QueryAsync<UserDTO>(new CommandDefinition("""
                select Id, UserName, HashedPassword, Email from `HospitalInformationSystemDB`.`users` where Email=@Email
                """, new { email }, cancellationToken: token));



            return result.SingleOrDefault();
        }
    }
}
