using System.Reflection;
using Infrastructure.Services;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
// Add services to the container.

builder.Services.AddControllers();

// Services
builder.Services.AddTransient<IUserService, UserService>();

// Mysql Connection
string mySqlConnectionStr = configuration.GetConnectionString("DefaultConnection");  
builder.Services.AddDbContext<SystemDBContext>(options =>
    options.UseMySql(mySqlConnectionStr, ServerVersion.AutoDetect(mySqlConnectionStr))
);


var logger = new LoggerConfiguration()
    .Enrich.WithProperty("Application", Assembly.GetEntryAssembly().GetName().Name)
    .Enrich.WithProperty("Version", Assembly.GetEntryAssembly().GetName().Version)
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "Local" || app.Environment.EnvironmentName == "Test")
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();