using HIS.Api.Auth;
using HIS.Api.Mappers;
using HIS.Application;
using HIS.Application.Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);
var conf = builder.Configuration;
builder.WebHost.UseUrls(new[] { "http://localhost:5000", "https://localhost:5050" }!);


builder.Services.AddControllers()
    .AddJsonOptions(opts =>
    {
        var enumConverter = new JsonStringEnumConverter();
        opts.JsonSerializerOptions.Converters.Add(enumConverter);
    });


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

builder.Services.AddAuthorization(x =>
{
    x.AddPolicy(AuthConstants.AdminPolicy, p => p.RequireClaim(AuthConstants.UserNameClaimType, 
        AuthConstants.AdminUserName));
    x.AddPolicy(AuthConstants.TrustedMemberPolicy, p => p.RequireAssertion(c =>
        c.User.HasClaim(x => x is { Type: AuthConstants.TrustedClaimType, Value: "true" }))); 
        // || c.User.HasClaim(x => x is { Type: AuthConstants.UserNameClaimType, Value: AuthConstants.AdminUserName })));
});

builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplication();
builder.Services.AddDatabase(conf["ConnectionStrings:MySqlConnectionString"]!);
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseMiddleware<ValidationErrorMappingMiddleware>();

var dbInitializer = app.Services.GetRequiredService<MySqlInitializer>();
await dbInitializer.InitializeAsync();

app.Run();
