using HIS.Application.Models;
using HIS.Contracts.Enums;
using HIS.Contracts.Requests;
using HIS.Contracts.Requests.Patients;
using HIS.Contracts.Responses;
using HIS.Contracts.Responses.Patients;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace HIS.Api.Mappers
{
    public static class PatientMapping
    {
        public static Patient MapToPatient(this CreatePatientRequest request, Guid specifiedUserId)
        {
            return new Patient
            {
                Id = specifiedUserId,
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


        public static PatientsResponse MapToResponses(this List<Patient> patients, int page, int pageSize, int patientsCount)
        {
            var response = new PatientsResponse
            {
                Items = patients.Select(x => x.MapToResponse()),
                Page = page,
                PageSize = pageSize,
                Total = patientsCount
            };
           
            return response;
        }

        public static PatientsResponseWithoutPagination MapToResponses(this List<Patient> patients)
        {
            return new PatientsResponseWithoutPagination
            {
                patientsResponse = patients.Select(x => x.MapToResponse()),
            };
        }


        public static GetAllPatientsOptions MapToOptions(this GetAllPatientsRequest request)
        {
            var options = new GetAllPatientsOptions
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Surname = request.Surname,
                DiseaseList = request.DiseaseList,
                Age = request.Age,
                SortField = request.SortBy?.Trim('+', '-'),
                SortOrder = request.SortBy == null ? SortOrder.Unsorted :
                    request.SortBy.StartsWith('-') ? SortOrder.Descending : SortOrder.Ascending,
                Page = request.Page.GetValueOrDefault(PagedRequest.DefaultPage),
                PageSize = request.PageSize.GetValueOrDefault(PagedRequest.DefaultPageSize),
            };

            options.FirstName = "%" + options.FirstName + "%";
            options.LastName = "%" + options.LastName + "%";
            options.Surname = "%" + options.Surname + "%";
            options.DiseaseList = "%" + options.DiseaseList + "%";

            return options;
        }

    }
}
