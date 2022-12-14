using Infrastructure.Context;
using Microsoft.Extensions.Logging;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Serilog.Context;

namespace Infrastructure.Services;

public interface ITestService
{
    Task<(bool isSuccess, int errorCode)> AddUsers();
    Task<(bool isSuccess, int errorCode)> AddCourses();
    Task<(bool isSuccess, int errorCode)> AddUserByCourse();
    Task<(bool isSuccess, int errorCode)> AddUserByClub();
    // Task<(bool isSuccess, int errorCode)> AddBestRecord();
}

public class TestService : ITestService 
{
    // Log 
    private readonly ILogger<TestService> _logger;
        
    // DB
    private readonly SystemDBContext _db;
    
    // Service
    public TestService(ILogger<TestService> logger, SystemDBContext db)
    {
        _logger = logger;

        _db = db;
    }
    
    private DateTime RandomDay()
    {
        Random random = new Random();

        DateTime start = new DateTime(2019, 1, 1);
        int range = (DateTime.Today - start).Days;           
        return start.AddDays(random.Next(range));
    }
    
    private string RandomPassword(int length)
    {
        Random random = new Random();
        
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    private int RandomInteger(int lowerBound, int upperBound)
    {
        Random random = new Random();
        
        var randomNumber = random.Next(lowerBound, upperBound);
        return randomNumber;
    }
    
    private double GetSecureDoubleWithinRange(double start, double end)
    {   
        Random random = new Random();
        
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "?????? ????????? ?????? ????????? ?????? ??? ?????? ??????");

            return (false, 1);
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "?????? ????????? ?????? ????????? ?????? ??? ?????? ??????");

            return (false, 2);
        }
    }

    public async Task<(bool isSuccess, int errorCode)> AddUserByCourse2()
    {
        try
        {
            var idCount = await _db.Users
                .AsNoTracking()
                .Where(p => p.Deleted == false)
                .CountAsync();
            
            for (var i = 1; i <= idCount ; i++) // ????????? ?????? ?????????
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

                var dataList = new List<UserByCourseModel>();

                int bestScore = 0;
                ulong bestScoreTime = 0;
                decimal longest = 0;
                ulong longestTime = 0;
                
                // ?????? ?????? ??? ??????
                for (var j = 0; j < 1000; j++) // 1000?????? ????????? ????????? ?????? ?????????
                {
                    var randomDate = RandomDay();
            
                    var nowUnixTime = (ulong) new DateTimeOffset(randomDate).ToUnixTimeMilliseconds();
            
                    var data = new UserByCourseModel()
                    {
                        UserId = i,
                        CourseId = RandomInteger(1, 51),
                        Score = RandomInteger(minScore, maxScore),
                        Longest = (decimal) GetSecureDoubleWithinRange(minLongest, maxLongest),
                        Updated = nowUnixTime,
                    };

                    if (bestScore > data.Score || bestScore == 0)
                    {
                        bestScore = data.Score;
                        bestScoreTime = data.Updated;
                    }
                    
                    if (data.Longest > longest )
                    {
                        longest = data.Longest;
                        longestTime = data.Updated;
                    }
                    
                    dataList.Add(data);
                }
                
                await _db.UsersByCourse.AddRangeAsync(dataList);
                
                var bestRecord = new UserBestRecordModel()
                { 
                    UserId = i,
                    Score = bestScore,
                    ScoreUpdated = bestScoreTime,
                    Longest = longest,
                    LongestUpdated = longestTime
                }; 
                
                await _db.UsersBestRecord.AddAsync(bestRecord);
            }
            
            await _db.SaveChangesAsync();

            return (true, 0);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "????????? ????????? ?????? ??????????????? ?????? ??? ???????????? ?????? ??????????????? ?????????"); 

            return (false, 3);
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
            
            for (var i = 1; i <= idCount ; i++) // ????????? ?????? ?????????
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

                var dataList = new List<UserByCourseModel>();

                // ????????? ???????????? ????????? ??????
                for (var j = 0; j < 1000; j++) // 1000?????? ????????? ????????? ?????? ?????????
                {
                    var randomDate = RandomDay();
            
                    var nowUnixTime = (ulong) new DateTimeOffset(randomDate).ToUnixTimeMilliseconds();
            
                    var data = new UserByCourseModel()
                    {
                        UserId = i,
                        CourseId = RandomInteger(1, 51),
                        Score = RandomInteger(minScore, maxScore),
                        Longest = (decimal) GetSecureDoubleWithinRange(minLongest, maxLongest),
                        Updated = nowUnixTime,
                    };
                    
                    dataList.Add(data);
                }

                var bestScore = (from data in dataList
                        orderby data.Score, data.Updated
                        select new
                        {
                            Score = data.Score,
                            ScoreUpdated = data.Updated
                        }
                    ).FirstOrDefault();
                
                var longest = (from data in dataList
                        orderby data.Longest descending , data.Updated
                        select new
                        {
                            data.Longest,
                            data.Updated
                        }
                    ).FirstOrDefault();

                var bestRecord = new UserBestRecordModel()
                { 
                    UserId = i,
                    Score = bestScore.Score,
                    ScoreUpdated = bestScore.ScoreUpdated,
                    Longest = longest.Longest,
                    LongestUpdated = longest.Updated
                }; 
            
                await _db.UsersBestRecord.AddAsync(bestRecord);

                await _db.UsersByCourse.AddRangeAsync(dataList);
            }
            
            await _db.SaveChangesAsync();

            return (true, 0);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "????????? ????????? ?????? ??????????????? ?????? ???  ???????????? ?????? ??????????????? ?????????"); 

            return (false, 3);
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
                var driverDistance = (decimal)GetSecureDoubleWithinRange(160.00, 300.00);
                var putDistance = (decimal)GetSecureDoubleWithinRange(1.00, 10.00);
                var swDistance = (decimal)GetSecureDoubleWithinRange(10.00, 30.00);
                var iron4Distance = (decimal)GetSecureDoubleWithinRange(140.00, 160.00);
                var iron5Distance = (decimal)GetSecureDoubleWithinRange(130.00, 150.00);
                var iron6Distance = (decimal)GetSecureDoubleWithinRange(120.00, 140.00);
                var iron7Distance = (decimal)GetSecureDoubleWithinRange(110.00, 130.00);
                var iron8Distance = (decimal)GetSecureDoubleWithinRange(100.00, 120.00);
                var iron9Distance = (decimal)GetSecureDoubleWithinRange(90.00, 110.00);
                
                foreach (var t in clubArray)
                {
                    var randomDate = RandomDay();

                    var nowUnixTime = (ulong) new DateTimeOffset(randomDate).ToUnixTimeMilliseconds();

                    decimal distance = 0;

                    switch (t)
                    {
                        case "driver":
                        {
                            distance = driverDistance;
                            break;
                        }
                        
                        case "put":
                        {
                            distance = putDistance;
                            break;
                        }
                        
                        case "sw":
                        {
                            distance = swDistance;
                            break;
                        } 
                        
                        case "iron4":
                        {
                            distance = iron4Distance;
                            break;
                        } 
                        
                        case "iron5":
                        {
                            distance = iron5Distance;
                            break;
                        } 
                        
                        case "iron6":
                        {
                            distance = iron6Distance;
                            break;
                        }
                        
                        case "iron7":
                        {
                            distance = iron7Distance;
                            break;
                        }
                        
                        case "iron8":
                        {
                            distance = iron8Distance;
                            break;
                        }
                        
                        case "iron9":
                        {
                            distance = iron9Distance;
                            break;
                        }
                    }

                    var club = new UserByClubModel()
                    {
                        UserId = i,
                        Club = t,
                        Distance = distance, 
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
            _logger.LogError(ex, "????????? ?????? ?????? ??????????????? ?????? ???  ???????????? ?????? ??????????????? ?????????"); 

            return (false, 4);
        }
    }

    // public async Task<(bool isSuccess, int errorCode)> AddBestRecord()
    // {
    //     try
    //     {
    //         var nowUnixTime = (ulong)DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    //     
    //         // var dataList = await _db.UsersByCourse
    //         //     .AsNoTracking()
    //         //     .GroupBy(p => p.UserId)
    //         //     .Select(d => new
    //         //     {
    //         //         UserId = d.Key,
    //         //         Score = d.Min(t => t.Score),
    //         //         Longest = d.Max(t => t.Longest),
    //         //     })
    //         //     .ToListAsync();
    //         
    //         // ?????? ????????? ?????? ?????????, ??????????????? ??????
    //         // ????????? ????????? ?????????????????? ?????? ???????????? ???????????? ??????
    //         // ?????? ?????? ????????? ????????? ???????????? ?????? ??????????????? ?????? ?????? ????????? ???????????? ????????????
    //         
    //         
    //         var dataList = await _db.UsersByCourse
    //             .AsNoTracking()
    //             .GroupBy(p => p.UserId)
    //             .Select(d => new
    //             {
    //                 UserId = d.Key,
    //                 BestScore = d.Min(t => t.Score),
    //                 Longest = d.Max(t => t.Longest),
    //             })
    //             .ToListAsync();
    //
    //
    //         var scoreJoinList = (from final in 
    //             join best in dataList
    //                 on new { final.UserId, final.Score } equals new { best.UserId, best.BestScore }
    //             select new
    //             {
    //                 Id = final.UserByCourseId,
    //                 UserId = best.UserId,
    //                 BestScore = final.Score,
    //                 ScoreUpdated = final.Updated
    //             });
    //         
    //         var longestJoinList = (from final in _db.UsersByCourse
    //             join best in dataList
    //                 on final.Longest equals best.Longest
    //             select new
    //             {
    //                 Id = final.UserByCourseId,
    //                 UserId = best.UserId,
    //                 Longest = best.Longest,
    //                 LongestUpdated = final.Updated
    //             }).ToListAsync();
    //             
    //         // foreach (var data in scoreJoinList)
    //         // {
    //         //     var bestRecord = new UserBestRecordModel()
    //         //     {
    //         //         
    //         //         // UserId = data.UserId,
    //         //         // Score = data.BestScore,
    //         //         // ScoreUpdated = nowUnixTime,
    //         //         // Longest = data.Longest,
    //         //         // LongestUpdated = nowUnixTime
    //         //     }; 
    //         //     
    //         //     await _db.UsersBestRecord.AddAsync(bestRecord);
    //         // }
    //         //
    //         await _db.SaveChangesAsync();
    //     
    //         return (true, 0);
    //     }
    //     catch (Exception ex)
    //     {
    //         Console.WriteLine(ex);
    //         return (false, 101);
    //     }
    // }
}