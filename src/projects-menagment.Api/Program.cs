using projects_menagment.Api.Middleware;
using projects_menagment.Application.DependencyInjection;
using projects_menagment.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
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
app.MapControllers();

app.MapGet("/api/test", () => new { message = "Hello from test endpoint", timestamp = DateTime.UtcNow })
    .WithName("GetTest")
    .WithOpenApi()
    .Produces<object>(StatusCodes.Status200OK);

app.Run();
