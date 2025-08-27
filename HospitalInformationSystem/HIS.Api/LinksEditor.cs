using HIS.Application.Models;
using HIS.Contracts.Requests.Patients;
using HIS.Contracts.Responses;
using HIS.Contracts.Responses.Patients;
using Microsoft.AspNetCore.Routing;

namespace HIS.Api
{
    public static class LinksEditor
    {
        public static IHttpContextAccessor? HttpContextAccessor { get; set; }

        public static void Configure(IHttpContextAccessor accessor)
        {
            HttpContextAccessor = accessor;
        }

        public static PatientsResponse AddLinksIntoResponse(this PatientsResponse patientsResponse, GetAllPatientsRequest request, LinkGenerator linkGenerator)
        {
            patientsResponse.Links.Add(new Link
            {
                Href = linkGenerator.GetPathByAction(HttpContextAccessor?.HttpContext!, "GetAllPatients", values: new GetAllPatientsRequest
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Surname = request.Surname,
                    DiseaseList = request.DiseaseList,
                    Age = request.Age,
                    Page = request.Page - 1,
                    PageSize = request.PageSize,
                    SortBy = request.SortBy,
                })!,
                Rel = "Self",
                Type = "GET",
                Name = "Previous page"
            });

            patientsResponse.Links.Add(new Link
            {
                Href = linkGenerator.GetPathByAction(HttpContextAccessor?.HttpContext!, "GetAllPatients", values: new GetAllPatientsRequest
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Surname = request.Surname,
                    DiseaseList = request.DiseaseList,
                    Age = request.Age,
                    Page = request.Page + 1,
                    PageSize = request.PageSize,
                    SortBy = request.SortBy,
                })!,
                Rel = "Self",
                Type = "GET",
                Name = "Next page"
            });

            return patientsResponse;
        }

        public static DoctorsResponse AddLinksIntoResponse(this DoctorsResponse doctorsResponse)
        {
            throw new NotImplementedException();
        }

    }
}
