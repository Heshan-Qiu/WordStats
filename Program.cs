using Microsoft.OpenApi.Models;
using WordStats;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Word Stats App API", Description = "Word Stats App API", Version = "v1" });
});

builder.Services.AddHostedService<WordStatsService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Word Stats App API v1"));

app.MapGet("/", () => "Hello World!");

app.Run();
