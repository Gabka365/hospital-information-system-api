using HIS.Api.Auth;
using HIS.Api.Mappers;
using HIS.Application.Services.Doctors;
using HIS.Application.Services.Patients;
using HIS.Contracts.Requests;
using HIS.Contracts.Requests.Patients;
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
                .WithName(Name)
                .RequireAuthorization(AuthConstants.AdminPolicy);

            return builder;
        }
    }
}
