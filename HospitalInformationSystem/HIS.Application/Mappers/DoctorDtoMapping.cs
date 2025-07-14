using HIS.Application.Models;
using HIS.Contracts.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIS.Application.DTOs;
using HIS.Contracts.Enums;

namespace HIS.Application.Mappers
{
    public static class DoctorDtoMapping
    {
        public static DoctorDto MapToDoctorDto(this Doctor doctor)
        {
            return new DoctorDto
            {
                Id = doctor.Id,
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                Surname = doctor.Surname,
                Specialties = string.Join(',', doctor.Specialties),
                Category = doctor.Category.ToString(),
                Experience = doctor.Experience
            };
        }

        public static Doctor MapToDoctor(this DoctorDto doctorDto)
        {
            var specs = doctorDto.Specialties.Split(',');
            var especs = new List<Speciality>();

            foreach (var spec in specs)
            {
                var espec = new Speciality();
                Enum.TryParse(spec, out espec);
                especs.Add(espec);   
            }

            var categ = new Category();
            Enum.TryParse(doctorDto.Category, out categ);

            return new Doctor
            {
                Id = doctorDto.Id,
                FirstName = doctorDto.FirstName,
                LastName = doctorDto.LastName,
                Surname = doctorDto.Surname,
                Specialties = especs,
                Category = categ,
                Experience = doctorDto.Experience
            };
        }
    }
}
