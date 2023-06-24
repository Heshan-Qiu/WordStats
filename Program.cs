using System.Text;
using Booster.CodingTest.Library;
using Microsoft.OpenApi.Models;
using WordStats;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Word Stats App API", Description = "Word Stats App API", Version = "v1" });
});

builder.Services.AddSingleton<Stream, WordStream>();
builder.Services.AddSingleton<Encoding, UTF8Encoding>();
builder.Services.AddSingleton<IWordStats, WordStatsDictionaryImpl>();
builder.Services.AddSingleton<IWordStatsWriter, WordStatsWriterConsoleImpl>();
builder.Services.Configure<WordStatsServiceOptions>(builder.Configuration.GetSection("WordStatsService"));
builder.Services.AddHostedService<WordStatsService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin();
        builder.AllowAnyMethod();
        builder.AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Word Stats App API v1"));

app.UseCors("AllowAll");

app.MapGet("/", () => "Hello WordStats App!");
app.MapGet("/stats", () => app.Services.GetService<IWordStats>()?.ToJsonString());

app.Run();
