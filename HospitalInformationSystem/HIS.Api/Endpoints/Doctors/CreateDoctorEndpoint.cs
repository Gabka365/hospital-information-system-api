using HIS.Api.Mappers;
using HIS.Application.Services.Doctors;
using HIS.Contracts.Requests.Doctors;
using Microsoft.AspNetCore.Http.HttpResults;
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

                    var doctor = request.MapToDoctor(specifiedUserId);
                    
                    var isCreated = await doctorService.CreateDoctorAsync(doctor, token);

                    if (!isCreated)
                    {
                        return Results.BadRequest();
                    }

                    var response = doctor.MapToResponse();

                    return TypedResults.Created(ApiEndpoints.V1.Doctors.Create, response);
                })
                .WithName(Name);

            return builder;
        }
    }
}
