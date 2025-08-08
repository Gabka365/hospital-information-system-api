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
                (Id, UserName, HashedPassword) values 
                (@Id, @UserName, @HashedPassword)
                """, userDto, cancellationToken: token));

            if (result != 1)
            {
                throw new Exception("Error with executing mysql query");
            }

            return userDto;
        }

        public async Task<UserDTO?> GetUserAsync(string UserName, CancellationToken token)
        {
            var connection = await _connector.CreateConnectionAsync(token);

            var result = await connection.QueryAsync<UserDTO>(new CommandDefinition("""
                select UserName, HashedPassword from `HospitalInformationSystemDB`.`users` where UserName=@UserName
                """, new { UserName }, cancellationToken: token));

            return result.SingleOrDefault();
        }

        public async Task<bool> VerifyUserExistingAsync(string hashedPassword, CancellationToken token)
        {
            var connection = await _connector.CreateConnectionAsync(token);

            var result = await connection.QueryAsync<UserDTO>(new CommandDefinition("""
                select * from `HospitalInformationSystemDB`.`users` where HashedPassword = @hashedPassword
                """, new { hashedPassword }, cancellationToken: token));

            return result.Count() == 1;
        }
    }
}
