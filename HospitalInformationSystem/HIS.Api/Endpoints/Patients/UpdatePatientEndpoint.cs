using HIS.Api.Auth;
using HIS.Api.Mappers;
using HIS.Application.Services.Doctors;
using HIS.Application.Services.Patients;
using HIS.Contracts.Requests;
using HIS.Contracts.Requests.Patients;
using HIS.Contracts.Responses;
using HIS.Contracts.Responses.Patients;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;

namespace HIS.Api.Endpoints.Patients
{
    public static class UpdatePatientEndpoint
    {
        public const string Name = "UpdatePatient";

        public static IEndpointRouteBuilder MapUpdatePatient(this IEndpointRouteBuilder builder)
        {
            builder.MapPut(ApiEndpoints.Patients.Update,
                async (Guid id, UpdatePatientRequest request, HttpContext context, IPatientService patientService,
                IOutputCacheStore outputCacheStore, CancellationToken token) =>
                {
                    var patient = request.MapToPatient(id);

                    var result = await patientService.UpdatePatientAsync(patient, token);

                    return TypedResults.Ok(result.MapToResponse());
                })
                .Produces<PatientsResponse>(StatusCodes.Status200OK)
                .Produces<ValidationErrorResponse>(StatusCodes.Status400BadRequest)
                .RequireAuthorization(AuthConstants.AdminPolicy)
                .WithName(Name);
    
            return builder;
        }
    }
}
