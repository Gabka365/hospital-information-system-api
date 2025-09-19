using Asp.Versioning;
using HIS.Api;
using HIS.Api.Auth;
using HIS.Api.Endpoints;
using HIS.Api.Health;
using HIS.Api.Mappers;
using HIS.Api.Swagger;
using HIS.Application;
using HIS.Application.Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);
var conf = builder.Configuration;
builder.WebHost.UseUrls(new[] { "http://localhost:5000", "https://localhost:5050" }!);
builder.Services.AddAuthentication(x =>
{
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(conf["Jwt:Key"]!)),
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidateAudience = false,
        ValidateIssuer = false
    };
});
builder.Services
    .AddHealthChecks()
    .AddCheck<DatabaseHealthCheck>(DatabaseHealthCheck.DatabaseName);
builder.Services.AddAuthorization(x =>
{
    x.AddPolicy(AuthConstants.AdminPolicy, p => p.RequireClaim(AuthConstants.UserNameClaimType,
        AuthConstants.AdminUserName));

    x.AddPolicy(AuthConstants.AdminPolicy,
        p => p.AddRequirements(new AdminAuthRequirement(conf["ApiKey"]!)));

    x.AddPolicy(AuthConstants.TrustedMemberPolicy, p => p.RequireAssertion(c =>
        c.User.HasClaim(x => x is { Type: AuthConstants.TrustedClaimType, Value: "true" }))); 
        // || c.User.HasClaim(x => x is { Type: AuthConstants.UserNameClaimType, Value: AuthConstants.AdminUserName })));
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddApiVersioning(x =>
{
    x.DefaultApiVersion = new ApiVersion(1.0);
    x.AssumeDefaultVersionWhenUnspecified = true;
    x.ApiVersionReader = new MediaTypeApiVersionReader("api-version");
    x.ReportApiVersions = true;
}).AddMvc().AddApiExplorer();
//builder.Services.AddControllers()
//    .AddJsonOptions(opts =>
//    {
//        var enumConverter = new JsonStringEnumConverter();
//        opts.JsonSerializerOptions.Converters.Add(enumConverter);
//    });
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen(x => x.OperationFilter<SwaggerDefaultValues>());
builder.Services.AddApplication();
builder.Services.AddDatabase(conf["ConnectionStrings:MySqlConnectionString"]!);
builder.Services.AddScoped<ApiKeyAuthFilter>();
builder.Services.AddOutputCache(c =>
{
    c.AddBasePolicy(p => p.Cache());
    c.AddPolicy(nameof(AuthCachePolicy), AuthCachePolicy.Instance);
});


var app = builder.Build();
LinksEditor.Configure(app.Services.GetRequiredService<IHttpContextAccessor>());
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(x =>
    {
        foreach (var description in app.DescribeApiVersions())
        {
            x.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName);
        }    
    });
}
app.UseAuthentication();
app.UseAuthorization();
app.MapHealthChecks("_health");
app.UseHttpsRedirection();
app.UseOutputCache();
app.UseResponseCaching();
//app.MapControllers();
app.AddApiEndpoints();
app.UseMiddleware<ValidationErrorMappingMiddleware>();

var dbInitializer = app.Services.GetRequiredService<MySqlInitializer>();
await dbInitializer.InitializeAsync();
app.Run();
