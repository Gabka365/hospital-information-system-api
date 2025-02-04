
var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls(new[] { "https://0.0.0.0:5050", "http://0.0.0.0:5060" });
var app = builder.Build();


app.MapGet("/", () => "Hello World!");
app.Run();
