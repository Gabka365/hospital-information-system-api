using HIS.Application.Database;
using HIS.Application.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace HIS.Application
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddSingleton<IDoctorRepository, DoctorRepository>();

            return services;
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
        {
            services.AddSingleton(new MySqlConnectionFactory(connectionString));
            services.AddSingleton<MySqlInitializer>();

            return services;
        }
    }
}

