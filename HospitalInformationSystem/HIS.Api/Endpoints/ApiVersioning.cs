using Asp.Versioning.Builder;
using Asp.Versioning;
using System.Runtime.CompilerServices;

namespace HIS.Api.Endpoints
{
    public static class ApiVersioning
    {
        public static ApiVersionSet VersionSet { get; private set; }

        public static IEndpointRouteBuilder CreateApiVersionSet(this IEndpointRouteBuilder builder)
        {
            VersionSet = builder.NewApiVersionSet()
                .HasApiVersion(new ApiVersion(1.0))
                .HasApiVersion(new ApiVersion(2.0))
                .ReportApiVersions()
                .Build();

            return builder;
        }
    }
}
