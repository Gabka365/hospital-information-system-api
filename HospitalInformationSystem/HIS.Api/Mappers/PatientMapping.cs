using HIS.Application.Models;
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
                Id = request.Id,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Surname = request.Surname,
                Age = request.Age,
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

    }
}
