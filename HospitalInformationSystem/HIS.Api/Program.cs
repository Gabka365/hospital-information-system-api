using HIS.Api.Auth;
using HIS.Api.Mappers;
using HIS.Application;
using HIS.Application.Database;
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
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplication();
builder.Services.AddDatabase(conf["ConnectionStrings:MySqlConnectionString"]!);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.UseMiddleware<ValidationErrorMappingMiddleware>();

var dbInitializer = app.Services.GetRequiredService<MySqlInitializer>();
await dbInitializer.InitializeAsync();

app.Run();
