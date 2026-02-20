var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/test", () => new { message = "Hello from test endpoint", timestamp = DateTime.UtcNow })
    .WithName("GetTest")
    .WithOpenApi()
    .Produces<object>(StatusCodes.Status200OK);


app.Run();
