using System.Security.Cryptography;
using Infrastructure.Context;
using Infrastructure.Models.Test;
using Microsoft.Extensions.Logging;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public interface ITestService
{
    Task<(bool isSuccess, int errorCode)> AddUsers();
    Task<(bool isSuccess, int errorCode)> AddCourses();
    Task<(bool isSuccess, int errorCode)> AddUserByCourse();
    Task<(bool isSuccess, int errorCode)> AddUserByClub();
    Task<(bool isSuccess, int errorCode)> AddBestRecord();
}

public class TestService : ITestService 
{
    // Log 
    private readonly ILogger<UserService> _logger;
        
    // DB
    private readonly SystemDBContext _db;
    
    // Service
    public TestService(ILogger<UserService> logger, SystemDBContext db)
    {
        _logger = logger;

        _db = db;
    }
    
    // TODO: [20221215-코드리뷰-3번] DI로 수명 주기는 설정하는 class의 경우 static 처리를 해서는 안됩니다. 메소드에 static을 모두 제거해주세요.
    
    
    // TODO: [20221215-코드리뷰-4번] 랜덤 변수를 멤버 변수가 아닌 지역 변수로 처리하도록 수정해주세요.
    private static Random random = new Random();
    
    /// <summary>
    /// DateTime to unix time 변환 함수
    /// </summary>
    /// <param name="dateTime">DateTime</param>
    /// <returns>unix time</returns>
    public static ulong DateTimeToUnixTime(DateTime dateTime)
    {
        ulong unixTimeStamp = (ulong)DateTimeOffset.Parse(dateTime.ToString("u")).ToUnixTimeMilliseconds();
            
        return unixTimeStamp;
    }

    /// <summary>
    /// unix time to DateTime 변환 함수
    /// </summary>
    /// <param name="unixTime">unix time</param>
    /// <returns>DateTime</returns>
    public static DateTime UnixTimeToDateTime(ulong unixTime)
    {
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            
        dateTime = dateTime.AddMilliseconds(unixTime).ToUniversalTime();

        return dateTime;
    }
    
    private static DateTime RandomDay()
    {
        DateTime start = new DateTime(2019, 1, 1);
        int range = (DateTime.Today - start).Days;           
        return start.AddDays(random.Next(range));
    }
    
    private static string RandomPassword(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    private static int RandomInteger(int lowerBound, int upperBound)
    {
        var randomNumber = random.Next(lowerBound, upperBound);
        return randomNumber;
    }
    
    private static double GetSecureDoubleWithinRange(double start, double end)
    {   
        return (random.NextDouble() * Math.Abs(end-start)) + start;
    }
    
    public async Task<(bool isSuccess, int errorCode)> AddUsers()
    {
        try
        {
            for (int i = 1; i <= 100; i++)
            {
                var randomDate = RandomDay();

                var nowUnixTime = (ulong) new DateTimeOffset(randomDate).ToUnixTimeMilliseconds();
                
                var user = new UserModel
                {
                    Username = $"user{i}",
                    Password = RandomPassword(10),
                    Name = $"user{i}",
                    Created = nowUnixTime, 
                    Updated = nowUnixTime, 
                };
                
                await _db.Users.AddAsync(user);
            }
            
            await _db.SaveChangesAsync();

            return (true, 0);
            
        }
        catch 
        {
            return (false, 100);
        }
    }

    public async Task<(bool isSuccess, int errorCode)> AddCourses()
    {
        try
        {
            for (int i = 1; i <= 50; i++)
            {
                var course = new CourseModel
                {
                    CourseName = $"course{i}",
                };
                
                await _db.Courses.AddAsync(course);
            }
            
            await _db.SaveChangesAsync();

            return (true, 0);
        }
        catch
        {
            return (false, 101);
        }
    }

    public async Task<(bool isSuccess, int errorCode)> AddUserByCourse()
    {
        try
        {
            var idCount = await _db.Users
                .AsNoTracking()
                .Where(p => p.Deleted == false)
                .CountAsync();
            
            for (var i = 1; i <= idCount ; i++) // 회원에 대한 반복문
            {
                var minScore = RandomInteger(60, 90);
                var scoreRange = RandomInteger(20, 40); 
                var maxScore = RandomInteger((minScore + scoreRange), 145);
            
                var minLongest = GetSecureDoubleWithinRange(160.00, 180.00);
                var longestRange = GetSecureDoubleWithinRange(30, 70);
                var maxLongest = GetSecureDoubleWithinRange((minLongest + longestRange), 300.00);
                
                // _logger.LogWarning($"User Id: {i}");
                // _logger.LogWarning($"Score Range: {minScore} ~ {maxScore}");
                // _logger.LogWarning($"Longest Range: {minLongest} ~ {maxLongest}");
                
                for (var j = 0; j < 1000; j++) // 1000개의 스코어 데이터 생성 반복문
                {
                    var randomDate = RandomDay();
            
                    var nowUnixTime = (ulong) new DateTimeOffset(randomDate).ToUnixTimeMilliseconds();
            
                    var course = new UserByCourseModel()
                    {
                        UserId = i,
                        CourseId = RandomInteger(1, 51),
                        Score = RandomInteger(minScore, maxScore),
                        Longest = (decimal) GetSecureDoubleWithinRange(minLongest, maxLongest),
                        Updated = nowUnixTime,
                    };
                
                    await _db.UsersByCourse.AddAsync(course);
                }
            }
            
            await _db.SaveChangesAsync();

            return (true, 0);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return (false, 101);
        }
    }
    
    public async Task<(bool isSuccess, int errorCode)> AddUserByClub()
    {
        try
        {
            var idCount = await _db.Users
                .AsNoTracking()
                .Where(p => p.Deleted == false)
                .CountAsync();

            string[] clubArray = { "driver", "put", "sw", "iron4", "iron5", "iron6", "iron7", "iron8", "iron9" };

            for (var i = 1; i <= idCount ; i++)
            {
                foreach (var t in clubArray)
                {
                    var randomDate = RandomDay();

                    var nowUnixTime = (ulong) new DateTimeOffset(randomDate).ToUnixTimeMilliseconds();

                    var club = new UserByClubModel()
                    {
                        UserId = i,
                        Club = t,
                        Distance = (decimal) GetSecureDoubleWithinRange(160.00, 300.00), // TODO: [20221215-코드리뷰-5번] 클럽별로 거리를 다르게 입력하도록 수정
                        Updated = nowUnixTime
                    };
                
                    await _db.UsersByClub.AddAsync(club);
                }
            }
            
            await _db.SaveChangesAsync();
    
            return (true, 0);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return (false, 101);
        }
    }
    
    public async Task<(bool isSuccess, int errorCode)> AddBestRecord()
    {
        try
        {
            var nowUnixTime = (ulong)DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        
            var dataList = await _db.UsersByCourse
                .AsNoTracking()
                .GroupBy(p => p.UserId)
                .Select(d => new
                {
                    UserId = d.Key,
                    Score = d.Min(t => t.Score),
                    Longest = d.Max(t => t.Longest),
                })
                .ToListAsync();
        
            foreach (var data in dataList)
            {
                var bestRecord = new UserBestRecordModel()
                {
                    UserId = data.UserId,
                    Score = data.Score,
                    ScoreUpdated = nowUnixTime,// TODO: [20221215-코드리뷰-6번] 스코어를 기록한 시간으로 입력이 되어야합니다.
                    Longest = data.Longest,
                    LongestUpdated = nowUnixTime// TODO: [20221215-코드리뷰-7번] 롱기스트를 기록한 시간으로 입력이 되어야합니다.
                }; 
                
                await _db.UsersBestRecord.AddAsync(bestRecord);
            }
            
            await _db.SaveChangesAsync();
        
            return (true, 0);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return (false, 101);
        }
    }
}