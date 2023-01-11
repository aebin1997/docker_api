using System.Reflection;
using Infrastructure.Services;
using Infrastructure.Context;
using Infrastructure.Services.DI;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson();

// Services DI
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<ITestService, TestService>();
builder.Services.AddTransient<ICourseService, CourseService>();
builder.Services.AddTransient<IStatisticsService, StatisticsService>();


builder.Services.AddTransient<IDiService, DiService>();
builder.Services.AddTransient<IDiTwoService, DiTwoService>();

builder.Services.AddSingleton<ISingletonService, SingletonService>(); // Application이 시작되고 종료될 때까지 1개의 객체를 유지
builder.Services.AddScoped<IScopedService, ScopedService>(); // 특정 세션(API로 설명하면 Request가 발생아되고 Response될 떄까지)이 종료될 때 까지 1개의 객체를 유지
builder.Services.AddTransient<ITransientService, TransientService>(); // 선언할 때 마다 객체를 새로 생성

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
if (app.Environment.IsDevelopment() || app.Environment.IsProduction() || app.Environment.IsStaging() || app.Environment.EnvironmentName == "Local")
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();