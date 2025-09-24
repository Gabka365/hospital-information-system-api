using HIS.Api.Auth;
using HIS.Api.Mappers;
using HIS.Application.Services.Doctors;
using HIS.Application.Services.Patients;
using HIS.Contracts.Requests;
using HIS.Contracts.Requests.Patients;
using HIS.Contracts.Responses;
using HIS.Contracts.Responses.Patients;
using Microsoft.AspNetCore.Mvc;

namespace HIS.Api.Endpoints.Patients
{
    public static class GetAllPatientsEndpoint
    {
        public const string Name = "GetAllPatients";

        public static IEndpointRouteBuilder MapGetAllPatients(this IEndpointRouteBuilder builder)
        {
            builder.MapGet(ApiEndpoints.Patients.GetAll,
                async ([AsParameters] GetAllPatientsRequest request, LinkGenerator linkGenerator,
                IPatientService patientService, HttpContext context, CancellationToken token) =>
                {
                    var options = request
                        .MapToOptions();

                    var patients = await patientService.GetAllPatientsAsync(options, token);

                    var patientsCount = await patientService.GetPatientsCountAsync(options, token);

                    var response = patients
                        .MapToResponses(
                        request.Page.GetValueOrDefault(PagedRequest.DefaultPage),
                        request.PageSize.GetValueOrDefault(PagedRequest.DefaultPageSize),
                        patientsCount)
                        .AddLinksIntoResponse(request, linkGenerator);

                    return TypedResults.Ok(response);
                })
                .Produces<PatientsResponse>(StatusCodes.Status200OK)
                .Produces<ValidationErrorResponse>(StatusCodes.Status400BadRequest)
                .RequireAuthorization(AuthConstants.AdminPolicy)
                .WithName($"{Name}V1")
                .WithApiVersionSet(ApiVersioning.VersionSet)
                .HasApiVersion(1.0)
                .WithMetadata(new ResponseCacheAttribute
                {
                    Duration = 30,
                    VaryByQueryKeys = new[] { "FirstName", "LastName", "Surname", "DiseaseList", "Age", "SortBy" },
                    VaryByHeader = "Accept, Accept-Encoding",
                    Location = ResponseCacheLocation.Client
                });

            return builder;
        }
    }
}
