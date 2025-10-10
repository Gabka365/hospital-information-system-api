using HIS.Api.Auth;
using HIS.Api.Mappers;
using HIS.Application.Services.Doctors;
using HIS.Contracts.Requests.Doctors;
using HIS.Contracts.Responses;
using HIS.Contracts.Responses.Doctors;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace HIS.Api.Endpoints.Doctors
{
    public static class CreateDoctorEndpoint
    {
        public const string Name = "CreateDoctor";

        public static IEndpointRouteBuilder MapCreateDoctor(this IEndpointRouteBuilder builder)
        {
            builder.MapPost(ApiEndpoints.V1.Doctors.Create,
                async (CreateDoctorRequest request, IDoctorService doctorService,
                IOutputCacheStore outputCacheStore, CancellationToken token) =>
                {
                    var specifiedUserId = await doctorService.GetUserIdByEmail(request.Email, token);

                    if (specifiedUserId is null)
                    {
                        return Results.BadRequest();
                    }

                    var doctor = request.MapToDoctor((Guid)specifiedUserId);

                    var isCreated = await doctorService.CreateDoctorAsync(doctor, token);

                    if (!isCreated)
                    {
                        return Results.BadRequest();
                    }

                    var response = doctor.MapToResponse();

                    return TypedResults.Created(ApiEndpoints.V1.Doctors.Create, response);
                })
                .Produces<DoctorResponse>(StatusCodes.Status200OK)
                .Produces<ValidationErrorResponse>(StatusCodes.Status400BadRequest)
                .RequireAuthorization(AuthConstants.AdminPolicy)
                .WithName($"{Name}V1")
                .WithApiVersionSet(ApiVersioning.VersionSet)
                .HasApiVersion(1.0)
                .RequireAuthorization()
                .WithMetadata(new ResponseCacheAttribute
                {
                    Duration = 30,
                    VaryByQueryKeys = new[] { "FirstName", "LastName", "Surname", "Experience", "Specialties", "Category", "Email" },
                    Location = ResponseCacheLocation.Client
                });

            return builder;
        }
    }
}
