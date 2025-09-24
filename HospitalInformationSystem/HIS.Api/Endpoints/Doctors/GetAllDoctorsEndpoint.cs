using HIS.Api.Auth;
using HIS.Api.Mappers;
using HIS.Application.Services.Doctors;
using HIS.Contracts.Requests;
using HIS.Contracts.Requests.Doctors;
using HIS.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace HIS.Api.Endpoints.Doctors
{
    public static class GetAllDoctorEndpoint
    {
        public const string Name = "GetAllDoctors";

        public static IEndpointRouteBuilder MapGetAllDoctors(this IEndpointRouteBuilder builder)
        {
            builder.MapGet(ApiEndpoints.V1.Doctors.GetAll,
                async ([AsParameters] GetAllDoctorsRequest request, LinkGenerator linkGenerator,
                IDoctorService doctorService, HttpContext context, CancellationToken token) =>
                {
                    var userId = context.GetUserId();

                    var options = request
                        .MapToOptions()
                        .WithUser(userId);

                    var doctors = await doctorService.GetAllDoctorsAsync(options, token);

                    var doctorsCount = await doctorService.GetDoctorsCountAsync(options, token);

                    var response = doctors
                        .MapToResponses(
                        request.Page.GetValueOrDefault(PagedRequest.DefaultPage),
                        request.PageSize.GetValueOrDefault(PagedRequest.DefaultPageSize),
                        doctorsCount)
                        .AddLinksIntoResponse(request, linkGenerator);

                    return TypedResults.Ok(response);
                })
                .Produces<DoctorsResponse>(StatusCodes.Status200OK)
                .Produces<ValidationErrorResponse>(StatusCodes.Status400BadRequest)
                .RequireAuthorization(AuthConstants.TrustedMemberPolicy)
                .WithName($"{Name}V1")
                .WithApiVersionSet(ApiVersioning.VersionSet)
                .HasApiVersion(1.0)
                .WithMetadata(new ResponseCacheAttribute
                {
                    Duration = 30,
                    VaryByQueryKeys = new[] { "FirstName", "LastName", "Surname", "Experience", "Specialties", "Category", "SortBy" },
                    Location = ResponseCacheLocation.Client
                });

            return builder;
        }
    }
}
