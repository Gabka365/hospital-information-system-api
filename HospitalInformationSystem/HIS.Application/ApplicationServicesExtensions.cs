using FluentValidation;
using HIS.Application.Database;
using HIS.Application.Repositories;
using HIS.Application.Repositories.Auth;
using HIS.Application.Repositories.Doctors;
using HIS.Application.Repositories.Patients;
using HIS.Application.Services.Auth;
using HIS.Application.Services.Doctors;
using HIS.Application.Services.Patients;
using HIS.Application.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace HIS.Application
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IDoctorRepository, DoctorRepository>();
            services.AddScoped<IDoctorService, DoctorService>();
            services.AddScoped<IPatientRepository, PatientRepository>();
            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddValidatorsFromAssemblyContaining<IApplicationMarker>(ServiceLifetime.Transient);

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

