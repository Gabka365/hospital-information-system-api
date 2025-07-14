using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Application.Database
{
    public class MySqlConnectionFactory 
    {
        private readonly string _connectionString;

        public MySqlConnectionFactory(string connectionString) 
        { 
            _connectionString = connectionString;
        }

        public async Task<MySqlConnection> CreateConnectionAsync()
        {
            var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }
    }
}
