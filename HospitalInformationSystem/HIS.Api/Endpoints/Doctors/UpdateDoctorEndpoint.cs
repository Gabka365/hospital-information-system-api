using HIS.Api.Auth;
using HIS.Api.Mappers;
using HIS.Application.Services.Doctors;
using HIS.Contracts.Requests;
using HIS.Contracts.Requests.Doctors;
using HIS.Contracts.Responses;
using HIS.Contracts.Responses.Doctors;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace HIS.Api.Endpoints.Doctors
{
    public static class UpdateDoctorEndpoint
    {
        public const string Name = "UpdateDoctor";

        public static IEndpointRouteBuilder MapUpdateDoctor(this IEndpointRouteBuilder builder)
        {
            builder.MapPut(ApiEndpoints.V1.Doctors.Update, 
                async (Guid id, UpdateDoctorRequest request, HttpContext context, IDoctorService doctorService, 
                IOutputCacheStore outputCacheStore, CancellationToken token) => 
                {
                    var userId = context.GetUserId();

                    var doctor = request.MapToDoctor(id);
                    var updatedDoctor = await doctorService.UpdateDoctorAsync(doctor, userId, token);
                    var response = updatedDoctor.MapToResponse();

                    return TypedResults.Ok(response);
                })
                .Produces<DoctorResponse>(StatusCodes.Status200OK)
                .Produces<ValidationErrorResponse>(StatusCodes.Status400BadRequest)
                .RequireAuthorization(AuthConstants.AdminPolicy)
                .WithName($"{Name}V1")
                .WithApiVersionSet(ApiVersioning.VersionSet)
                .HasApiVersion(1.0)
                .WithMetadata(new ResponseCacheAttribute
                {
                    Duration = 30,
                    VaryByQueryKeys = new[] { "FirstName", "LastName", "Surname", "Specialties", "Category", "Experience" },
                    VaryByHeader = "Accept, Accept-Encoding",
                    Location = ResponseCacheLocation.Client
                });

            return builder;
        }
    }
}
