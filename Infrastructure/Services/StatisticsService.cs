using Infrastructure.Context;
using Infrastructure.Models.Statistics;
using Infrastructure.Models.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace Infrastructure.Services;

public interface IStatisticsService
{
    Task<(bool isSuccess, int errorCode, GetUserScoreRangeByCourseResponse response)> GetUserScoreRangeByCourse(int userId);
    Task<(bool isSuccess, int errorCode, GetUserLongestRangeByCourseResponse response)> GetUserLongestRangeByCourse(int userId);
    Task<(bool isSuccess, int errorCode, GetCourseRoundingCountResponse response)> GetCourseRoundingCount(int courseId);
    Task<(bool isSuccess, int errorCode, GetCourseRoundingCountByYearResponse response)> GetCourseRoundingCountByYear(int courseId);
    Task<(bool isSuccess, int errorCode, GetCourseRoundingCountByMonthResponse response)> GetCourseRoundingCountByMonth(int courseId);
    Task<(bool isSuccess, int errorCode, GetBestScoreByCourseResponse response)> GetBestScoreByCourse();
    Task<(bool isSuccess, int errorCode, GetLongestByCourseResponse response)> GetLongestByCourse();
}

public class StatisticsService : IStatisticsService
{
    // Log 
    private readonly ILogger<StatisticsService> _logger;
        
    // DB
    private readonly SystemDBContext _testDB;
        
    // Service
    public StatisticsService(ILogger<StatisticsService> logger, SystemDBContext testDB)
    {
        _logger = logger;

        _testDB = testDB;
    }

    // public static DateTime UnixTimeToDateTime(ulong unixTime)
    // {
    //     DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
    //         
    //     dateTime = dateTime.AddMilliseconds(unixTime).ToUniversalTime();
    //
    //     int month = dateTime.Month;
    //     
    //     return dateTime;
    // }
    
    public static int UnixTimeToMonth(ulong unixTime)
    {
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            
        dateTime = dateTime.AddMilliseconds(unixTime).ToUniversalTime();

        int month = dateTime.Month;
        
        return month;
    }
    
    public static int UnixTimeToYear(ulong unixTime)
    {
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            
        dateTime = dateTime.AddMilliseconds(unixTime).ToUniversalTime();

        int year = dateTime.Year;
        
        return year;
    }
    
    public async Task<(bool isSuccess, int errorCode, GetUserScoreRangeByCourseResponse response)> GetUserScoreRangeByCourse(int userId)
    {
        try
        {
            var data = await _testDB.UsersByCourse
                .AsNoTracking()
                .GroupBy(p => p.UserId)
                .Where(p => p.Key == 1)
                .Select(p => new
                {
                    a = _testDB.UsersByCourse.Count(p => p.Score >= 60 && p.Score < 70),
                    b = _testDB.UsersByCourse.Count(p => p.Score >= 70 && p.Score < 80),
                    c = _testDB.UsersByCourse.Count(p => p.Score >= 80 && p.Score < 90),
                    d = _testDB.UsersByCourse.Count(p => p.Score >= 90 && p.Score < 100),
                    e = _testDB.UsersByCourse.Count(p => p.Score >= 100 && p.Score < 110),
                    f = _testDB.UsersByCourse.Count(p => p.Score >= 110 && p.Score < 120),
                    g = _testDB.UsersByCourse.Count(p => p.Score >= 120 && p.Score < 130),
                    h = _testDB.UsersByCourse.Count(p => p.Score >= 130 && p.Score < 140),
                    i = _testDB.UsersByCourse.Count(p => p.Score >= 140 && p.Score < 145)
                })
                .FirstOrDefaultAsync();

            var response = new GetUserScoreRangeByCourseResponse
            {
                A = data.a,
                B = data.b,
                C = data.c,
                D = data.d,
                E = data.e,
                F = data.f,
                G = data.g,
                H = data.h,
                I = data.i,
                // UserId = userId,
                // Range = (from range in data
                //         select new GetUserScoreRangeByCourseItem
                //         {
                //             A = data.a,
                //             B = data.b,
                //             C = data.c,
                //             D = data.d,
                //             E = data.e,
                //             F = data.f,
                //             G = data.g,
                //             H = data.h,
                //             I = data.i,
                //         }).ToList()
            };

            return (true, 0, response);
        }
        catch (Exception ex)
        {
            using (LogContext.PushProperty("JsonData", new
                   {
                       
                   }))
            {
                _logger.LogError(ex, "노출중인 팝업 조회 중 오류 발생");
            }

            return (false, 0, null);
        }
    }

    public async Task<(bool isSuccess, int errorCode, GetUserLongestRangeByCourseResponse response)> GetUserLongestRangeByCourse(int userId)
    {
        try
        {
            var data = await _testDB.UsersByCourse
                .AsNoTracking()
                .GroupBy(p => p.UserId)
                .Where(p => p.Key == 1)
                .Select(p => new
                {
                    a = _testDB.UsersByCourse.Count(p => p.Longest >= 160 && p.Longest < 100),
                    b = _testDB.UsersByCourse.Count(p => p.Longest >= 100 && p.Score < 140),
                    c = _testDB.UsersByCourse.Count(p => p.Longest >= 140 && p.Longest < 180),
                    d = _testDB.UsersByCourse.Count(p => p.Longest >= 180 && p.Longest < 220),
                    e = _testDB.UsersByCourse.Count(p => p.Longest >= 220 && p.Longest < 260),
                    f = _testDB.UsersByCourse.Count(p => p.Longest >= 260 && p.Longest < 300),
                })
                .FirstOrDefaultAsync();

            var response = new GetUserLongestRangeByCourseResponse
            {
                A = data.a,
                B = data.b,
                C = data.c,
                D = data.d,
                E = data.e,
                F = data.f
            };

            return (true, 0, response);
        }
        catch (Exception ex)
        {
            using (LogContext.PushProperty("JsonData", new
                   {
                       
                   }))
            {
                _logger.LogError(ex, "노출중인 팝업 조회 중 오류 발생");
            }

            return (false, 0, null);
        }
    }

    public async Task<(bool isSuccess, int errorCode, GetCourseRoundingCountResponse response)> GetCourseRoundingCount(int courseId)
    {
        try
        {
            var data = await _testDB.UsersByCourse
                .AsNoTracking()
                .GroupBy(p => p.CourseId)
                .Where(p => p.Key == 1)
                .Select(p => new
                {
                    CourseId = p.Key,
                    Count = p.Count()
                })
                .FirstOrDefaultAsync();

            var response = new GetCourseRoundingCountResponse
            {
                CourseId = data.CourseId,
                Count = data.Count
            };

            return (true, 0, response);
        }
        catch (Exception ex)
        {
            using (LogContext.PushProperty("JsonData", new
                   {
                       
                   }))
            {
                _logger.LogError(ex, "노출중인 팝업 조회 중 오류 발생");
            }

            return (false, 0, null);
        }
    }

    public async Task<(bool isSuccess, int errorCode, GetCourseRoundingCountByYearResponse response)> GetCourseRoundingCountByYear(int courseId)
    {
        try
        {
            // var start2018 = (ulong) new DateTimeOffset(DateTime.Parse("2018-01-01T00:00:00")).ToUnixTimeMilliseconds();
            // var end2018 = (ulong) new DateTimeOffset(DateTime.Parse("2018-12-31T23:59:59")).ToUnixTimeMilliseconds();
            // var start2019 = (ulong) new DateTimeOffset(DateTime.Parse("2019-01-01T00:00:00")).ToUnixTimeMilliseconds();
            // var end2019 = (ulong) new DateTimeOffset(DateTime.Parse("2019-12-31T23:59:59")).ToUnixTimeMilliseconds();
            // var start2020 = (ulong) new DateTimeOffset(DateTime.Parse("2020-01-01T00:00:00")).ToUnixTimeMilliseconds();
            // var end2020 = (ulong) new DateTimeOffset(DateTime.Parse("2020-12-31T23:59:59")).ToUnixTimeMilliseconds();
            // var start2021 = (ulong) new DateTimeOffset(DateTime.Parse("2021-01-01T00:00:00")).ToUnixTimeMilliseconds();
            // var end2021 = (ulong) new DateTimeOffset(DateTime.Parse("2021-12-31T23:59:59")).ToUnixTimeMilliseconds();
            // var start2022 = (ulong) new DateTimeOffset(DateTime.Parse("2022-01-01T00:00:00")).ToUnixTimeMilliseconds();
            // var end2022 = (ulong) new DateTimeOffset(DateTime.Parse("2022-12-31T23:59:59")).ToUnixTimeMilliseconds();
            
            // var data = await _testDB.UsersByCourse
            //     .AsNoTracking()
            //     .GroupBy(p => p.CourseId)
            //     .Where(p => p.Key == 1)
            //     .Select(p => new
            //     {
            //         a = _testDB.UsersByCourse.Count(p => p.Updated >= start2018 && p.Updated < end2018),
            //         b = _testDB.UsersByCourse.Count(p => p.Updated >= start2019 && p.Updated < end2019),
            //         c = _testDB.UsersByCourse.Count(p => p.Updated >= start2020 && p.Updated < end2020),
            //         d = _testDB.UsersByCourse.Count(p => p.Updated >= start2021 && p.Updated < end2021),
            //         e = _testDB.UsersByCourse.Count(p => p.Updated >= start2022 && p.Updated < end2022),
            //     })
            //     .FirstOrDefaultAsync();

            var dataList = _testDB.UsersByCourse
                .AsNoTracking()
                .Where(p => p.CourseId == 1)
                .Select(p => new DateTimeResponse
                {
                    UserId = p.UserId,
                    CourseId = p.CourseId,
                    Score = p.Score,
                    Longest = p.Longest,
                    Updated = UnixTimeToYear(p.Updated)
                })
                .ToList();
            
            var monthList = dataList
                .GroupBy(p => p.Updated)
                .Select(p => new
                {
                    Year = p.Key,
                    Count = p.Count()
                })
                .OrderBy(p => p.Year)
                .ToList();

            var response = new GetCourseRoundingCountByYearResponse
            {
                CourseId = courseId,
                List = (from year in monthList
                    select new GetCourseRoundingCountByYearListItem
                    {
                        Year = year.Year,
                        Count = year.Count
                    }).ToList()
            };
            
            return (true, 0, response);
        }
        catch (Exception ex)
        {
            using (LogContext.PushProperty("JsonData", new
                   {
                       
                   }))
            {
                _logger.LogError(ex, "노출중인 팝업 조회 중 오류 발생");
            }

            return (false, 0, null);
        }
    }

    public async Task<(bool isSuccess, int errorCode, GetCourseRoundingCountByMonthResponse response)> GetCourseRoundingCountByMonth(int courseId)
    {
        try
        {
            var dataList = _testDB.UsersByCourse
                .AsNoTracking()
                .Where(p => p.CourseId == 1)
                .Select(p => new DateTimeResponse
                {
                    UserId = p.UserId,
                    CourseId = p.CourseId,
                    Score = p.Score,
                    Longest = p.Longest,
                    Updated = UnixTimeToMonth(p.Updated)
                    // Updated = UnixTimeToDateTime(p.Updated)
                })
                .ToList();

            // var monthList = from month in dataList
            //     group month by month.UpdatedMonth
            //     into grp
            //     select new
            //     {
            //         Month = grp.Key,
            //         Count = grp.Count()
            //     }

            var monthList = dataList
                .GroupBy(p => p.Updated)
                .Select(p => new
                {
                    Month = p.Key,
                    Count = p.Count()
                })
                .OrderBy(p => p.Month)
                .ToList();

            var response = new GetCourseRoundingCountByMonthResponse
            {
                CourseId = courseId,
                List = (from month in monthList
                    select new GetCourseRoundingCountByMonthListItem
                    {
                        Month = month.Month,
                        Count = month.Count
                    }).ToList()
            };

            return (true, 0, response);
        }
        catch (Exception ex)
        {
            using (LogContext.PushProperty("JsonData", new
                   {
                       
                   }))
            {
                _logger.LogError(ex, "노출중인 팝업 조회 중 오류 발생");
            }

            return (false, 0, null);
        }
    }

    public async Task<(bool isSuccess, int errorCode, GetBestScoreByCourseResponse response)> GetBestScoreByCourse()
    {
        try
        {
            var dataList = await _testDB.UsersByCourse
                .AsNoTracking()
                .GroupBy(p => p.CourseId)
                .Select(p => new
                {
                    CourseId = p.Key,
                    MaxScore = p.Max(m => m.Score)
                }).ToListAsync();

            var response = new GetBestScoreByCourseResponse
            {
                List = (from data in dataList
                    select new GetBestScoreByCourseItem
                    {
                        CourseId = data.CourseId,
                        MaxScore = data.MaxScore
                    }).ToList()
            };

            return (true, 0, response);
        }
        catch (Exception ex)
        {
            using (LogContext.PushProperty("JsonData", new
                   {
                       
                   }))
            {
                _logger.LogError(ex, "노출중인 팝업 조회 중 오류 발생");
            }

            return (false, 0, null);
        }
    }

    public async Task<(bool isSuccess, int errorCode, GetLongestByCourseResponse response)> GetLongestByCourse()
    {
        try
        {
            var dataList = await _testDB.UsersByCourse
                .AsNoTracking()
                .GroupBy(p => p.CourseId)
                .Select(p => new
                {
                    CourseId = p.Key,
                    Longest = p.Max(m => m.Longest)
                }).ToListAsync();

            var response = new GetLongestByCourseResponse
            {
                List = (from data in dataList
                    select new GetLongestByCourseItem
                    {
                        CourseId = data.CourseId,
                        Longest = data.Longest
                    }).ToList()
            };

            return (true, 0, response);
        }
        catch (Exception ex)
        {
            using (LogContext.PushProperty("JsonData", new
                   {
                       
                   }))
            {
                _logger.LogError(ex, "노출중인 팝업 조회 중 오류 발생");
            }

            return (false, 0, null);
        }
    }
}