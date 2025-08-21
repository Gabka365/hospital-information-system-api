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
    public static class DoctorDTOMapping
    {
        public static DoctorDTO MapToDoctorDTO(this Doctor doctor)
        {
            return new DoctorDTO
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

        public static Doctor MapToDoctor(this DoctorDTO DoctorDTO)
        {
            var specs = DoctorDTO.Specialties.Split(',');
            var especs = new List<Speciality>();

            foreach (var spec in specs)
            {
                var espec = new Speciality();
                Enum.TryParse(spec, out espec);
                especs.Add(espec);   
            }

            var categ = new Category();
            Enum.TryParse(DoctorDTO.Category, out categ);

            return new Doctor
            {
                Id = DoctorDTO.Id,
                FirstName = DoctorDTO.FirstName,
                LastName = DoctorDTO.LastName,
                Surname = DoctorDTO.Surname,
                Specialties = especs,
                Category = categ,
                Experience = DoctorDTO.Experience,
                Rating = DoctorDTO.Rating,
                UserRating = DoctorDTO.UserRating
            };
        }
    }
}
