using System.Reflection;
using Infrastructure.Context;
using Infrastructure.Models.Statistics;
using Infrastructure.Models.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Serilog.Context;

namespace Infrastructure.Services;

public interface IStatisticsService
{
    Task<(bool isSuccess, int errorCode, GetUserScoreRangeByCourseResponse response)> GetUserScoreRangeByCourse(GetUserScoreRangeByCourseRequest request);
    Task<(bool isSuccess, int errorCode, GetUserLongestRangeByCourseResponse response)> GetUserLongestRangeByCourse(GetUserLongestRangeByCourseRequest request);
    Task<(bool isSuccess, int errorCode, GetCourseRoundingCountResponse response)> GetCourseRoundingCount(GetCourseRoundingCountRequest request);
    Task<(bool isSuccess, int errorCode, GetCourseRoundingCountByYearResponse response)> GetCourseRoundingCountByYear(GetCourseRoundingCountByYearRequest request);
    Task<(bool isSuccess, int errorCode, GetCourseRoundingCountByMonthResponse response)> GetCourseRoundingCountByMonth(GetCourseRoundingCountByMonthRequest request);
    Task<(bool isSuccess, int errorCode, GetBestScoreByCourseResponse response)> GetBestScoreByCourse(GetBestScoreByCourseRequest request);
    Task<(bool isSuccess, int errorCode, GetLongestByCourseResponse response)> GetLongestByCourse(GetLongestByCourseRequest request);
}

public class StatisticsService : IStatisticsService
{
    // Log 
    private readonly ILogger<StatisticsService> _logger;
        
    // DB
    private readonly SystemDBContext _db;
        
    // Service
    public StatisticsService(ILogger<StatisticsService> logger, SystemDBContext db)
    {
        _logger = logger;

        _db = db;
    }

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
    
    public async Task<(bool isSuccess, int errorCode, GetUserScoreRangeByCourseResponse response)> GetUserScoreRangeByCourse(GetUserScoreRangeByCourseRequest request)
    {
        try
        {
            if (request.Page <= 0) 
            {
                using (LogContext.PushProperty("LogProperty", new 
                       {
                           request = JObject.FromObject(request)
                       }))
                {
                    _logger.LogInformation("특정 회원의 코스별 스코어 범위 카운트: 페이지 값이 정수가 아님");
                }
                
                return (false, 30001, null);
            }
            
            if (request.PageSize is not (10 or 20 or 50)) 
            {
                using (LogContext.PushProperty("LogProperty", new 
                       {
                           request = JObject.FromObject(request)
                       }))
                {
                    _logger.LogInformation("특정 회원의 코스별 스코어 범위 카운트: 페이지 사이즈가 범위를 벋어남");
                }
                
                return (false, 30002, null);
            }
            
            var query = _db.UsersByCourse
                .AsNoTracking()
                .Where(p => p.UserId == request.UserId);

            if (request.CourseId != null)
            {
                query = query.Where(p => request.CourseId.Contains(p.CourseId));
            }
            
            var dataPageList = await query
                .GroupBy(p => p.CourseId)
                .OrderByDescending(p => p.Key)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(p => new GetUserScoreRangeByCourseItem
                {
                    CourseId = p.Key,
                    ScoreRange = new ScoreRangeItem
                    {
                        Score60To69 = p.Count(p => p.Score >= 60 && p.Score < 70),
                        Score70To79 = p.Count(p => p.Score >= 70 && p.Score < 80),
                        Score80To89 = p.Count(p => p.Score >= 80 && p.Score < 90),
                        Score90To99 = p.Count(p => p.Score >= 90 && p.Score < 100),
                        Score100To109 = p.Count(p => p.Score >= 100 && p.Score < 110),
                        Score110To119 = p.Count(p => p.Score >= 110 && p.Score < 120),
                        Score120To129 = p.Count(p => p.Score >= 120 && p.Score < 130),
                        Score130To139 = p.Count(p => p.Score >= 130 && p.Score < 140),
                        Score140To144 = p.Count(p => p.Score >= 140 && p.Score < 145)
                    }
                }).ToListAsync();

            var response = new GetUserScoreRangeByCourseResponse
            {
                UserId = request.UserId,
                CourseScoreRangeList = dataPageList
            };

            return (true, 0, response);
        }
        catch (Exception ex)
        {
            using (LogContext.PushProperty("JsonData", new
                   {
                       request = JObject.FromObject(request)
                   }))
            {
                _logger.LogError(ex, "특정 회원의 코스별 스코어 범위 카운트 조회 중 오류 발생");
            }

            return (false, 3000, null);
        }
    }

    public async Task<(bool isSuccess, int errorCode, GetUserLongestRangeByCourseResponse response)> GetUserLongestRangeByCourse(GetUserLongestRangeByCourseRequest request)
    {
        try
        {
            if (request.Page <= 0) 
            {
                using (LogContext.PushProperty("LogProperty", new 
                       {
                           request = JObject.FromObject(request)
                       }))
                {
                    _logger.LogInformation("회원 별 최고 기록: 페이지 값이 정수가 아님");
                }
                
                return (false, 30011, null);
            }
            
            if (request.PageSize is not (10 or 20 or 50)) 
            {
                using (LogContext.PushProperty("LogProperty", new 
                       {
                           request = JObject.FromObject(request)
                       }))
                {
                    _logger.LogInformation("회원 별 최고 기록: 페이지 사이즈가 범위를 벋어남");
                }
                
                return (false, 30012, null);
            }
            
            var query = _db.UsersByCourse
                .AsNoTracking()
                .Where(p => p.UserId == request.UserId);

            if (request.CourseId != null)
            {
                query = query.Where(p => request.CourseId.Contains(p.CourseId));
            }
            
            var dataPageList = await query
                .GroupBy(p => p.CourseId)
                .OrderByDescending(p => p.Key)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(p => new GetUserLongestRangeByCourseItem
                {
                    CourseId = p.Key,
                    LongestRange = new LongestRangeItem
                    {
                        Longest160To179 = p.Count(p => p.Longest >= 160 && p.Longest < 180),
                        Longest180To199 = p.Count(p => p.Longest >= 180 && p.Score < 200),
                        Longest200To219 = p.Count(p => p.Longest >= 200 && p.Longest < 220),
                        Longest220To239 = p.Count(p => p.Longest >= 220 && p.Longest < 240),
                        Longest240To259 = p.Count(p => p.Longest >= 240 && p.Longest < 260),
                        Longest260To279 = p.Count(p => p.Longest >= 260 && p.Longest < 280),
                        Longest280To299 = p.Count(p => p.Longest >= 280 && p.Longest < 300),
                    }
                }).ToListAsync();
            
            var response = new GetUserLongestRangeByCourseResponse
            {
                UserId = request.UserId,
                CourseLongestRangeList = dataPageList
            };

            return (true, 0, response);
        }
        catch (Exception ex)
        {
            using (LogContext.PushProperty("JsonData", new
                   {
                       request = JObject.FromObject(request) 
                   }))
            {
                _logger.LogError(ex, "특정 회원의 코스별 롱기스트 거리 범위 카운트 조회 중 오류 발생");
            }

            return (false, 3001, null);
        }
    }

    public async Task<(bool isSuccess, int errorCode, GetCourseRoundingCountResponse response)> GetCourseRoundingCount(GetCourseRoundingCountRequest request)
    {
        try
        {
            if (request.Page <= 0) 
            {
                using (LogContext.PushProperty("LogProperty", new 
                       {
                           request = JObject.FromObject(request)
                       }))
                {
                    _logger.LogInformation("회원 별 최고 기록: 페이지 값이 정수가 아님");
                }
                
                return (false, 30021, null);
            }
            
            if (request.PageSize is not (10 or 20 or 50)) 
            {
                using (LogContext.PushProperty("LogProperty", new 
                       {
                           request = JObject.FromObject(request)
                       }))
                {
                    _logger.LogInformation("회원 별 최고 기록: 페이지 사이즈가 범위를 벋어남");
                }
                
                return (false, 30022, null);
            }
            
            var query = _db.UsersByCourse
                .AsNoTracking();

            if (request.CourseId != null)
            {
                query = query.Where(p => request.CourseId.Contains(p.CourseId));
            }
            
            var dataPageList = await query
                .GroupBy(p => p.CourseId)
                .OrderByDescending(p => p.Key)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(p => new GetCourseRoundingCountItem
                {
                    CourseId = p.Key,
                    Count = p.Count()
                }).ToListAsync();

            var response = new GetCourseRoundingCountResponse
            {
                RoundingCountList = dataPageList
            };

            return (true, 0, response);
        }
        catch (Exception ex)
        {
            using (LogContext.PushProperty("JsonData", new
                   {
                       request = JObject.FromObject(request)
                   }))
            {
                _logger.LogError(ex, "특정 코스의 라운딩 카운트 조회 중 오류 발생");
            }

            return (false, 3002, null);
        }
    }

    public async Task<(bool isSuccess, int errorCode, GetCourseRoundingCountByYearResponse response)> GetCourseRoundingCountByYear(GetCourseRoundingCountByYearRequest request)
    {
        try
        {
            if (request.Page <= 0) 
            {
                using (LogContext.PushProperty("LogProperty", new 
                       {
                           request = JObject.FromObject(request)
                       }))
                {
                    _logger.LogInformation("회원 별 최고 기록: 페이지 값이 정수가 아님");
                }
                
                return (false, 30031, null);
            }
            
            if (request.PageSize is not (10 or 20 or 50)) 
            {
                using (LogContext.PushProperty("LogProperty", new 
                       {
                           request = JObject.FromObject(request)
                       }))
                {
                    _logger.LogInformation("회원 별 최고 기록: 페이지 사이즈가 범위를 벋어남");
                }
                
                return (false, 30032, null);
            }

            var query = _db.UsersByCourse
                .AsNoTracking();

            if (request.CourseId != null)
            {
                query = query.Where(p => request.CourseId.Contains(p.CourseId));
            }

            var convertToYear = await query
                .Select(p => new 
                {
                    UserId = p.UserId,
                    CourseId = p.CourseId,
                    Score = p.Score,
                    Longest = p.Longest,
                    UpdatedYear = UnixTimeToYear(p.Updated)
                }).ToListAsync();

            var dataList = convertToYear;

            if (request.YearRangeStart != null && request.YearRangeEnd != null)
            {
                dataList = dataList.Where(p => p.UpdatedYear >= request.YearRangeStart && p.UpdatedYear <= request.YearRangeEnd).ToList();
            }
               
            var dataPageList = dataList
                .GroupBy(p => p.CourseId)
                .OrderByDescending(p => p.Key)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(p => new GetCourseRoundingCountByYearListItem
                {
                    CourseId = p.Key,
                    Count = p.GroupBy(p => p.UpdatedYear)
                        .Select(s => new  
                        {
                            Year = s.Key,
                            Count = s.Count()
                        }).ToDictionary(t => t.Year, t=> t.Count)
                })
                .ToList();

            var response = new GetCourseRoundingCountByYearResponse
            {
                RoundingCountList = dataPageList
            };

            return (true, 0, response);
        }
        catch (Exception ex)
        {
            using (LogContext.PushProperty("JsonData", new
                   {
                       request = JObject.FromObject(request)
                   }))
            {
                _logger.LogError(ex, "특정 코스의 연도별 라운딩 카운트 조회 중 오류 발생");
            }

            return (false, 3003, null);
        }
    }

    public async Task<(bool isSuccess, int errorCode, GetCourseRoundingCountByMonthResponse response)> GetCourseRoundingCountByMonth(GetCourseRoundingCountByMonthRequest request)
    {
        try
        {
            if (request.Page <= 0) 
            {
                using (LogContext.PushProperty("LogProperty", new 
                       {
                           request = JObject.FromObject(request)
                       }))
                {
                    _logger.LogInformation("회원 별 최고 기록: 페이지 값이 정수가 아님");
                }
                
                return (false, 30041, null);
            }
            
            if (request.PageSize is not (10 or 20 or 50)) 
            {
                using (LogContext.PushProperty("LogProperty", new 
                       {
                           request = JObject.FromObject(request)
                       }))
                {
                    _logger.LogInformation("회원 별 최고 기록: 페이지 사이즈가 범위를 벋어남");
                }
                
                return (false, 30042, null);
            }
            
            var query = _db.UsersByCourse
                .AsNoTracking();

            if (request.CourseId != null)
            {
                query = query.Where(p => request.CourseId.Contains(p.CourseId));
            }

            var convertToYearMonth = await query
                .Select(p => new
                {
                    UserId = p.UserId,
                    CourseId = p.CourseId,
                    Score = p.Score,
                    Longest = p.Longest,
                    UpdatedYear = UnixTimeToYear(p.Updated),
                    UpdatedMonth = UnixTimeToMonth(p.Updated)
                })
                .ToListAsync();

            var dataList = convertToYearMonth;
            
            if (request.YearRangeStart != null && request.YearRangeEnd != null)
            {
                dataList = dataList.Where(p => p.UpdatedYear >= request.YearRangeStart && p.UpdatedYear <= request.YearRangeEnd).ToList();
            }
            
            if (request.MonthRangeStart != null && request.MonthRangeEnd != null)
            {
                dataList = dataList.Where(p => p.UpdatedMonth >= request.MonthRangeStart && p.UpdatedMonth <= request.MonthRangeEnd).ToList();
            }
            
            var dataPageList = dataList
                .GroupBy(p => p.CourseId)
                .OrderByDescending(p => p.Key)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(p => new GetCourseRoundingCountByMonthListItem
                {
                    CourseId = p.Key,
                    Count = p.GroupBy(p => p.UpdatedYear)
                        .Select(s => new
                        {
                            Year = s.Key,
                            MonthList = s.GroupBy(s => s.UpdatedMonth)
                                .Select(t => new
                                {
                                    Month = t.Key,
                                    Count = t.Count()
                                }).ToDictionary(p => p.Month, p=> p.Count)
                        }).ToDictionary(p => p.Year, p => p.MonthList)
                })
                .ToList();

            var response = new GetCourseRoundingCountByMonthResponse
            {
                RoundingCountList = dataPageList
            };

            return (true, 0, response);
        }
        catch (Exception ex)
        {
            using (LogContext.PushProperty("JsonData", new
                   {
                       request = JObject.FromObject(request)
                   }))
            {
                _logger.LogError(ex, "특정 코스의 월별 라운딩 카운트 조회 중 오류 발생");
            }

            return (false, 3004, null);
        }
    }

    public async Task<(bool isSuccess, int errorCode, GetBestScoreByCourseResponse response)> GetBestScoreByCourse(GetBestScoreByCourseRequest request)
    {
        try
        {
            if (request.Page <= 0) 
            {
                using (LogContext.PushProperty("LogProperty", new 
                       {
                           request = JObject.FromObject(request)
                       }))
                {
                    _logger.LogInformation("회원 별 최고 기록: 페이지 값이 정수가 아님");
                }
                
                return (false, 30051, null);
            }
            
            if (request.PageSize is not (10 or 20 or 50)) 
            {
                using (LogContext.PushProperty("LogProperty", new 
                       {
                           request = JObject.FromObject(request)
                       }))
                {
                    _logger.LogInformation("회원 별 최고 기록: 페이지 사이즈가 범위를 벋어남");
                }
                
                return (false, 30052, null);
            }

            var query = _db.UsersByCourse
                .AsNoTracking();

            if (request.CourseId != null)
            {
                query = query.Where(p => request.CourseId.Contains(p.CourseId));
            }
            
            var dataPageList = await query
                .GroupBy(p => p.CourseId)
                .OrderByDescending(p => p.Key)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(p => new GetBestScoreByCourseItem
                {
                    CourseId = p.Key,
                    BestScore= p.Min(m => m.Score)
                }).ToListAsync();
                
            var response = new GetBestScoreByCourseResponse
            {
                CourseBestScoreList = dataPageList
            };

            return (true, 0, response);
        }
        catch (Exception ex)
        {
            using (LogContext.PushProperty("JsonData", new
                   {
                       request = JObject.FromObject(request)
                   }))
            {
                _logger.LogError(ex, "코스별로 최고 스코어 데이터 조회 중 오류 발생");
            }

            return (false, 3005, null);
        }
    }

    public async Task<(bool isSuccess, int errorCode, GetLongestByCourseResponse response)> GetLongestByCourse(GetLongestByCourseRequest request)
    {
        try
        {
            if (request.Page <= 0) 
            {
                using (LogContext.PushProperty("LogProperty", new 
                       {
                           request = JObject.FromObject(request)
                       }))
                {
                    _logger.LogInformation("회원 별 최고 기록: 페이지 값이 정수가 아님");
                }
                
                return (false, 30061, null);
            }
            
            if (request.PageSize is not (10 or 20 or 50)) 
            {
                using (LogContext.PushProperty("LogProperty", new 
                       {
                           request = JObject.FromObject(request)
                       }))
                {
                    _logger.LogInformation("회원 별 최고 기록: 페이지 사이즈가 범위를 벋어남");
                }
                
                return (false, 30062, null);
            }
            
            var query = _db.UsersByCourse
                .AsNoTracking();

            if (request.CourseId != null)
            {
                query = query.Where(p => request.CourseId.Contains(p.CourseId));
            }
            
            var dataPageList = await query
                .GroupBy(p => p.CourseId)
                .OrderByDescending(p => p.Key)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(p => new GetLongestByCourseItem
                {
                    CourseId = p.Key,
                    Longest= p.Max(m => m.Longest)
                }).ToListAsync();
                
            var response = new GetLongestByCourseResponse
            {
                CourseLongestList = dataPageList
            };

            return (true, 0, response);
        }
        catch (Exception ex)
        {
            using (LogContext.PushProperty("JsonData", new
                   {
                       request = JObject.FromObject(request)
                   }))
            {
                _logger.LogError(ex, "코스별로 최고 롱기스트 거리 데이터 조회 중 오류 발생");
            }

            return (false, 3006, null);
        }
    }
}