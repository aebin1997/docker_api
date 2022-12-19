using System.Reflection;
using Infrastructure.Services;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllers();

// Services DI
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<ITestService, TestService>();
builder.Services.AddTransient<ICourseService, CourseService>();
builder.Services.AddTransient<IStatisticsService, StatisticsService>();

// Mysql Connection

// appsettings 파일 수정 사항
// TODO: [20221215-코드리뷰-1번-확인] testDB라는 명칭으로 설정했던 DB 이름을 ParkAebinDB로 변경 (MySQL에서도 새로 생성하시기 바랍니다.)
// -> 변경이 안되어있어서 리뷰 진행중 변경

// TODO: [20221215-코드리뷰-2번-확인] ParkAebinDB의 사용자를 새로 생성하여 연결하도록 수정 (root 계정을 사용하는게 아닌 새로운 사용자로 연결을 시도해야합니다.) -> database 폴더에 SQL파일 생성해주세요.
// -> mysql 사용자 생성 쿼리 파일은 생성하였지만 connection string을 변경해놓지 않아 리뷰 진행중 변경  

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