using HIS.Application.DTOs;
using HIS.Application.Models;
using HIS.Application.Repositories;
using HIS.Application.Repositories.Auth;
using HIS.Application.Repositories.Doctors;
using HIS.Application.Repositories.Patients;
using HIS.Application.Repositories.Ratings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Application.Seed
{
    public class Seed
    {

        public static async Task FillAsync(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            using var serviceScope = serviceProvider.CreateScope();

            await FillUsersAsync(serviceScope, configuration);
            await FillDoctorsAsync(serviceScope, configuration);
            await FillPatientsAsync(serviceScope, configuration);
            await FillRatingsAsync(serviceScope, configuration);
            await FillPatientsDoctorsAsync(serviceScope, configuration);
        }

        private static async Task FillUsersAsync(IServiceScope serviceScope, IConfiguration configuration)
        {
            var authRepository = serviceScope.ServiceProvider.GetRequiredService<IAuthRepository>();
            var usersData = configuration.GetSection("Seed:Auth");

            foreach (var userData in usersData.GetChildren())
            {
                var userDto = new UserDTO();
                userData.Bind(userDto);

                await authRepository!.CreateUserAsync(userDto);
            }
        }

        private static async Task FillDoctorsAsync(IServiceScope serviceScope, IConfiguration configuration)
        {
            var doctorRepository = serviceScope.ServiceProvider.GetService<IDoctorRepository>();
            var doctorsData = configuration.GetSection("Seed:Doctors");

            foreach (var userData in doctorsData.GetChildren())
            {
                var doctorDto = new DoctorDTO();
                userData.Bind(doctorDto);
                await doctorRepository!.CreateDoctorAsync(doctorDto);
            }
        }

        private static async Task FillPatientsAsync(IServiceScope serviceScope, IConfiguration configuration)
        {
            var patientRepository = serviceScope.ServiceProvider.GetService<IPatientRepository>();
            var patientsData = configuration.GetSection("Seed:Patients");

            foreach (var userData in patientsData.GetChildren())
            {
                var patientDto = new PatientDTO();
                userData.Bind(patientDto);

                await patientRepository!.CreatePatientAsync(patientDto);
            }
        }

        private static async Task FillRatingsAsync(IServiceScope serviceScope, IConfiguration configuration)
        {
            var ratingsRepository = serviceScope.ServiceProvider.GetService<IRatingsRepository>();
            var usersData = configuration.GetSection("Seed:Auth").GetChildren();
            var doctorsData = configuration.GetSection("Seed:Doctors").GetChildren();

            foreach(var (userData, doctorData) in usersData.Zip(doctorsData))
            {
                var userDto = new UserDTO();
                var doctorDto = new DoctorDTO();
                userData.Bind(userDto);
                doctorData.Bind(doctorDto);

                await ratingsRepository!.RateDoctorAsync(doctorDto.Id, 6, userDto.Id);
            }
        }

        private static async Task FillPatientsDoctorsAsync(IServiceScope serviceScope, IConfiguration configuration)
        {
            var doctorRepository = serviceScope.ServiceProvider.GetService<IDoctorRepository>();
            var patientsData = configuration.GetSection("Seed:Patients").GetChildren();
            var doctorsData = configuration.GetSection("Seed:Doctors").GetChildren();

            foreach (var (patientData, doctorData) in patientsData.Zip(doctorsData))
            {
                var patientDto = new PatientDTO();
                var doctorDto = new DoctorDTO();
                patientData.Bind(patientDto);
                doctorData.Bind(doctorDto);

                await doctorRepository!.AddPatientForDoctorAsync(patientDto.Id, doctorDto.Id);
            }
        }
    }
}
