using projects_menagment.Api.Middleware;
using projects_menagment.Application.DependencyInjection;
using projects_menagment.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
var frontendOrigin = builder.Configuration["Frontend:Origin"] ?? "http://localhost:5173";

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy.WithOrigins(frontendOrigin)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("FrontendPolicy");
app.MapControllers();

app.MapGet("/api/test", () => new { message = "Hello from test endpoint", timestamp = DateTime.UtcNow })
    .WithName("GetTest")
    .WithOpenApi()
    .Produces<object>(StatusCodes.Status200OK);

app.Run();

public partial class Program;
