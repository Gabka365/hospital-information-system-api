using HIS.Api.Auth;
using HIS.Api.Mappers;
using HIS.Application.Services.Doctors;
using HIS.Application.Services.Patients;
using HIS.Contracts.Requests.Patients;
using HIS.Contracts.Responses;
using HIS.Contracts.Responses.Patients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace HIS.Api.Endpoints.Patients
{
    public static class CreatePatientEndpoint
    {
        public const string Name = "CreatePatient";

        public static IEndpointRouteBuilder MapCreatePatient(this IEndpointRouteBuilder builder)
        {
            builder.MapPost(ApiEndpoints.Patients.Create,
                async (CreatePatientRequest request, IPatientService patientService,
                IOutputCacheStore outputCacheStore, CancellationToken token) =>
                {
                    var specifiedUserId = await patientService.GetUserIdByEmail(request.Email, token);

                    var doctor = request.MapToPatient(specifiedUserId);

                    var patient = await patientService.CreatePatientAsync(doctor, token);

                    if (patient is null)
                    {
                        return Results.BadRequest();
                    }

                    var response = doctor.MapToResponse();

                    return TypedResults.Created(ApiEndpoints.Patients.Create, response);
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
                    VaryByQueryKeys = new[] { "FirstName", "LastName", "Surname", "DiseaseList", "Age", "Email" },
                    VaryByHeader = "Accept, Accept-Encoding",
                    Location = ResponseCacheLocation.Client
                });

            return builder;
        }
    }
}
