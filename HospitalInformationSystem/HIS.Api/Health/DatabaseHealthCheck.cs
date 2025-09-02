using HIS.Application.Database;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Runtime.CompilerServices;

namespace HIS.Api.Health
{
    public class DatabaseHealthCheck : IHealthCheck
    {
        private readonly MySqlConnectionFactory _connectionFactory;
        private readonly ILogger<DatabaseHealthCheck> _logger;
        public const string DatabaseName = "his-api-db";

        public DatabaseHealthCheck(MySqlConnectionFactory connectionFactory, ILogger<DatabaseHealthCheck> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new())
        {
            try
            {
                _ = await _connectionFactory.CreateConnectionAsync(cancellationToken);
                return HealthCheckResult.Healthy();
            }
            catch (Exception ex)
            {
                const string errmsg = "Database is unhealthy";
                _logger.LogError(errmsg, ex);
                return HealthCheckResult.Unhealthy(errmsg, ex);
            }
        }
    }
}
