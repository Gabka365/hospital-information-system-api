
using blog.Presentation;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls(new[] { "https://0.0.0.0:5050", "http://0.0.0.0:5060" });
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins("http://192.168.0.105:3000")
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
});

var app = builder.Build();

app.UseCors("AllowReactApp");
app.MapControllers();

app.Run();
