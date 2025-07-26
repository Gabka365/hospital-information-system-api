using HIS.Application.Models;
using HIS.Contracts.Enums;
using HIS.Contracts.Requests.Patients;
using HIS.Contracts.Responses.Patients;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace HIS.Api.Mappers
{
    public static class PatientMapping 
    {
        public static Patient MapToPatient(this CreatePatientRequest request)
        {
            return new Patient
            {
                Id = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Surname = request.Surname,
                Age = request.Age,
                DiseaseList = request.DiseaseList
                .Split(',')
                .Select(x =>
                (DiseaseList)Enum.Parse(typeof(DiseaseList), x))
                .ToList()
            };
        }

        public static Patient MapToPatient(this UpdatePatientRequest request, Guid id)
        {
            return new Patient
            {
                Id = id,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Surname = request.Surname,
                Age = request.Age,
                DiseaseList = request.DiseaseList
                .Split(',')
                .Select(x =>
                (DiseaseList)Enum.Parse(typeof(DiseaseList), x))
                .ToList()
            };
        }

        public static PatientResponse MapToResponse(this Patient patient)
        {
            return new PatientResponse
            {
                Id = patient.Id,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                Surname = patient.Surname,
                Age = patient.Age,
                DiseaseList = patient.DiseaseList
            };
        }


        public static PatientsResponse MapToResponses(this List<Patient> patients)
        {
            return new PatientsResponse
            {
               patientsResponse = patients.Select(x => x.MapToResponse()).ToList()
            };
        }

    }
}
