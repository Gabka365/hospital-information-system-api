using HIS.Application.DTOs;
using HIS.Application.Models;
using HIS.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Application.Mappers
{
    public static class PatientDtoMapping
    {
        public static PatientDTO MapToPatientDto(this Patient patient)
        {
            return new PatientDTO
            {
                Id = patient.Id,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                Surname = patient.Surname,
                Age = patient.Age,
                DiseaseList = string.Join(',', patient.DiseaseList.Select(x => x.ToString()))
            };
        }

        public static Patient MapToPatient(this PatientDTO patientDto)
        {
            return new Patient
            {
                Id = patientDto.Id,
                FirstName = patientDto.FirstName,
                LastName = patientDto.LastName,
                Surname= patientDto.Surname,
                Age= patientDto.Age,
                DiseaseList = patientDto.DiseaseList
                .Split(',')
                .Select(x =>
                (DiseaseList)Enum.Parse(typeof(DiseaseList), x))
                .ToList()
            };
        }

    }
}
