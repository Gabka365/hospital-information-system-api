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

        public async Task FillAsync(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            using var serviceScope = serviceProvider.CreateScope();
            
            Task.WaitAll(new Task[]
            {
                FillUsersAsync(serviceScope, configuration),
                FillDoctorsAsync(serviceScope, configuration),
                FillPatientsAsync(serviceScope, configuration),
                FillPatientsAsync(serviceScope, configuration),
                FillRatingsAsync(serviceScope, configuration),
                FillPatientsDoctorsAsync(serviceScope, configuration)
            });
        }

        private async Task FillUsersAsync(IServiceScope serviceScope, IConfiguration configuration)
        {
            var authRepository = serviceScope.ServiceProvider.GetService<AuthRepository>();
            var usersData = configuration.GetSection("Seed:Auth");

            foreach (var userData in usersData.GetChildren())
            {
                var userDto = new UserDTO();
                userData.Bind(userDto);

                await authRepository!.CreateUserAsync(userDto);
            }
        }

        private async Task FillDoctorsAsync(IServiceScope serviceScope, IConfiguration configuration)
        {
            var doctorRepository = serviceScope.ServiceProvider.GetService<DoctorRepository>();
            var doctorsData = configuration.GetSection("Seed:Doctors");

            foreach (var userData in doctorsData.GetChildren())
            {
                var doctorDto = new DoctorDTO();
                userData.Bind(doctorDto);
                await doctorRepository!.CreateDoctorAsync(doctorDto);
            }
        }

        private async Task FillPatientsAsync(IServiceScope serviceScope, IConfiguration configuration)
        {
            var patientRepository = serviceScope.ServiceProvider.GetService<PatientRepository>();
            var patientsData = configuration.GetSection("Seed:Patients");

            foreach (var userData in patientsData.GetChildren())
            {
                var patientDto = new PatientDTO();
                userData.Bind(patientDto);

                await patientRepository!.CreatePatientAsync(patientDto);
            }
        }

        private async Task FillRatingsAsync(IServiceScope serviceScope, IConfiguration configuration)
        {
            var ratingsRepository = serviceScope.ServiceProvider.GetService<RatingsRepository>();
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

        private async Task FillPatientsDoctorsAsync(IServiceScope serviceScope, IConfiguration configuration)
        {
            var doctorRepository = serviceScope.ServiceProvider.GetService<DoctorRepository>();
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
