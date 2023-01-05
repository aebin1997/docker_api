using System.Diagnostics;
using Infrastructure.Context;
using Infrastructure.Models.Statistics;
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
                    _logger.LogInformation("특정 회원의 코스별 스코어 범위 카운트: 페이지 사이즈가 범위를 벗어남");
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
                    _logger.LogInformation("특정 회원의 코스 별 롱기스트 거리 범위 카운트: 페이지 값이 정수가 아님");
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
                    _logger.LogInformation("특정 회원의 코스 별 롱기스트 거리 범위 카운트: 페이지 사이즈가 범위를 벋어남");
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
                        Longest180To199 = p.Count(p => p.Longest >= 180 && p.Longest < 200),
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
                _logger.LogError(ex, "특정 회원의 코스 별 롱기스트 거리 범위 카운트 조회 중 오류 발생");
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
                    _logger.LogInformation("특정 코스의 라운딩 카운트: 페이지 값이 정수가 아님");
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
                    _logger.LogInformation("특정 코스의 라운딩 카운트: 페이지 사이즈가 범위를 벋어남");
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
                    _logger.LogInformation("특정 코스의 연도별 라운딩 카운트: 페이지 값이 정수가 아님");
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
                    _logger.LogInformation("특정 코스의 연도별 라운딩 카운트: 페이지 사이즈가 범위를 벋어남");
                }
                
                return (false, 30032, null);
            }

            if (request.YearRangeStart > request.YearRangeEnd)
            {
                using (LogContext.PushProperty("LogProperty", new 
                       {
                           request = JObject.FromObject(request)
                       }))
                {
                    _logger.LogInformation("특정 코스의 연도별 라운딩 카운트: 년도 범위 오류");
                }
                
                return (false, 30033, null);
            }

            var query = _db.UsersByCourse
                .AsNoTracking();

            if (request.CourseId != null)
            {
                query = query.Where(p => request.CourseId.Contains(p.CourseId));
            }

            if (request.YearRangeStart != null && request.YearRangeEnd != null)
            {
                var searchStartUnixTime = (ulong) new DateTimeOffset(request.YearRangeStart.Value, 1, 1, 0, 0, 0, TimeSpan.Zero).ToUnixTimeMilliseconds();
                var searchEndUnixTime = (ulong) new DateTimeOffset((request.YearRangeEnd.Value + 1), 1, 1, 0, 0, 0, TimeSpan.Zero).ToUnixTimeMilliseconds();
                
                query = query.Where(p => p.Updated >= searchStartUnixTime && p.Updated < searchEndUnixTime);
            }

            var result = await query.ToListAsync();
            
            var dataPageList = result
                .Select(p => new
                {
                    CourseId = p.CourseId,
                    UpdatedYear = UnixTimeHandler.UnixTimeToYear(p.Updated)
                })
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
                        })
                        .OrderByDescending(p => p.Year)
                        .ToDictionary(t => t.Year, t=> t.Count)
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

    public async Task<(bool isSuccess, int errorCode, GetCourseRoundingCountByMonthResponse response)> GetCourseRoundingCountByMonth3(GetCourseRoundingCountByMonthRequest request)
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
                    _logger.LogInformation("특정 코스의 월별 라운딩 카운트: 페이지 값이 정수가 아님");
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
                    _logger.LogInformation("특정 코스의 월별 라운딩 카운트: 페이지 사이즈가 범위를 벋어남");
                }
                
                return (false, 30042, null);
            }
            
            if (request.YearRangeStart > request.YearRangeEnd)
            {
                using (LogContext.PushProperty("LogProperty", new 
                       {
                           request = JObject.FromObject(request)
                       }))
                {
                    _logger.LogInformation("특정 코스의 월별 라운딩 카운트: 년도 범위 오류");
                }
                
                return (false, 30043, null);
            }

            var query = _db.UsersByCourse
                .AsNoTracking();

            if (request.CourseId != null)
            {
                query = query.Where(p => request.CourseId.Contains(p.CourseId));
            }
            
            if (request.YearRangeStart != null && request.YearRangeEnd != null && request.MonthRangeStart != null && request.MonthRangeEnd != null)
            {
                var searchStartUnixTime = (ulong) new DateTimeOffset(request.YearRangeStart.Value, request.MonthRangeStart.Value, 1, 0, 0, 0, TimeSpan.Zero).ToUnixTimeMilliseconds();

                var yearRangeEnd = request.YearRangeEnd.Value;
                var monthRangeEnd = request.MonthRangeEnd.Value;

                if (monthRangeEnd == 12)
                {
                    yearRangeEnd += 1;
                    monthRangeEnd = 1;
                }
                else
                {
                    monthRangeEnd += 1;
                }

                var searchEndUnixTime = (ulong) new DateTimeOffset(yearRangeEnd, monthRangeEnd, 1, 0, 0, 0, TimeSpan.Zero).ToUnixTimeMilliseconds();
                
                query = query.Where(p => p.Updated >= searchStartUnixTime && p.Updated < searchEndUnixTime);
            }
            
            // TODO: [20221228-코드리뷰-44번] 코스 연도별 라운딩 통계 조회 로직처럼 수정을 진행해보세요. - done

            var result = await query.ToListAsync();

            var dataPageList = result
                .Select(p => new
                {
                    CourseId = p.CourseId,
                    UpdatedYear = UnixTimeHandler.UnixTimeToYear(p.Updated),
                    UpdatedMonth = UnixTimeHandler.UnixTimeToMonth(p.Updated)
                })
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
                                })
                                .OrderByDescending(t => t.Month)
                                .ToDictionary(p => p.Month, p=> p.Count)
                        })
                        .OrderByDescending(p => p.Year)
                        .ToDictionary(p => p.Year, p => p.MonthList)
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

    public async Task<(bool isSuccess, int errorCode, GetCourseRoundingCountByMonthResponse response)> GetCourseRoundingCountByMonth2(GetCourseRoundingCountByMonthRequest request)
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
                    _logger.LogInformation("특정 코스의 월별 라운딩 카운트: 페이지 값이 정수가 아님");
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
                    _logger.LogInformation("특정 코스의 월별 라운딩 카운트: 페이지 사이즈가 범위를 벋어남");
                }
                
                return (false, 30042, null);
            }
            
            if (request.YearRangeStart > request.YearRangeEnd)
            {
                using (LogContext.PushProperty("LogProperty", new 
                       {
                           request = JObject.FromObject(request)
                       }))
                {
                    _logger.LogInformation("특정 코스의 월별 라운딩 카운트: 년도 범위 오류");
                }
                
                return (false, 30043, null);
            }

            var query = _db.UsersByCourse
                .AsNoTracking();

            if (request.CourseId != null)
            {
                query = query.Where(p => request.CourseId.Contains(p.CourseId));
            }
            
            if (request.YearRangeStart != null && request.YearRangeEnd != null && request.MonthRangeStart != null && request.MonthRangeEnd != null)
            {
                var searchStartUnixTime = (ulong) new DateTimeOffset(request.YearRangeStart.Value, request.MonthRangeStart.Value, 1, 0, 0, 0, TimeSpan.Zero).ToUnixTimeMilliseconds();

                var yearRangeEnd = request.YearRangeEnd.Value;
                var monthRangeEnd = request.MonthRangeEnd.Value;

                if (monthRangeEnd == 12)
                {
                    yearRangeEnd += 1;
                    monthRangeEnd = 1;
                }
                else
                {
                    monthRangeEnd += 1;
                }

                var searchEndUnixTime = (ulong) new DateTimeOffset(yearRangeEnd, monthRangeEnd, 1, 0, 0, 0, TimeSpan.Zero).ToUnixTimeMilliseconds();
                
                query = query.Where(p => p.Updated >= searchStartUnixTime && p.Updated < searchEndUnixTime);
            }
            
            var result = await query.ToListAsync();

            var stopWatch = new Stopwatch();
            stopWatch.Start();
            
            var dataPageList = result
                .Select(p => new
                {
                    CourseId = p.CourseId,
                    UpdatedYear = UnixTimeHandler.UnixTimeToYear(p.Updated),
                    UpdatedMonth = UnixTimeHandler.UnixTimeToMonth(p.Updated)
                })
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
                                })
                                .OrderByDescending(t => t.Month)
                                .ToDictionary(p => p.Month, p=> p.Count)
                        })
                        .OrderByDescending(p => p.Year)
                        .ToDictionary(p => p.Year, p => p.MonthList)
                })
                .ToList();

            stopWatch.Stop();
            
            _logger.LogDebug($"dataPageList process time: {stopWatch.ElapsedMilliseconds}ms");

            // var dictList = new List<Tuple<int?, List<Tuple<int?, int>>>>();
            var dictList = new List<TestList>();
            var startYear = request.YearRangeStart;
                
            for (var i = 0; i <= (request.YearRangeEnd - request.YearRangeStart); i++)
            {
                var months = new List<MonthList>();

                if (startYear == request.YearRangeStart)
                {
                    for (var j = request.MonthRangeStart; j <= 12; j++)
                    {
                        {
                            months.Add(new MonthList(j, 0));
                        }
                    }
                }
                else if(startYear == request.YearRangeEnd)
                {
                    for (var j = 1; j <= request.MonthRangeEnd; j++)
                    {
                        months.Add(new MonthList(j, 0));
                    }
                }
                else
                {
                    for (var j = 1; j <= 12; j++)
                    {
                        months.Add(new MonthList(j, 0));
                    } 
                }

                dictList.Add(new TestList(startYear, months));

                startYear += 1;
            }
            
            var dataPageList2 = result
                .Select(p => new
                {
                    CourseId = p.CourseId,
                    UpdatedYear = UnixTimeHandler.UnixTimeToYear(p.Updated),
                    UpdatedMonth = UnixTimeHandler.UnixTimeToMonth(p.Updated)
                })
                .GroupBy(p => p.CourseId)
                .OrderByDescending(p => p.Key)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(p => new GetCourseRoundingCountByMonthListItem2
                {
                    CourseId = p.Key,
                    Count = p.GroupBy(p => p.UpdatedYear)
                        .Select(s => new TestList
                        {
                            Year = s.Key,
                            MonthList = s.GroupBy(s => s.UpdatedMonth)
                                .Select(t => new MonthList
                                {
                                    Month = t.Key,
                                    Count = t.Count()
                                }).ToList()
                        }).ToList()
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

    public async Task<(bool isSuccess, int errorCode, GetCourseRoundingCountByMonthResponse response)> GetCourseRoundingCountByMonth(GetCourseRoundingCountByMonthRequest request)
    {
        try
        {
            #region 유효성 검사
            
            if (request.Page <= 0) 
            {
                using (LogContext.PushProperty("LogProperty", new 
                       {
                           request = JObject.FromObject(request)
                       }))
                {
                    _logger.LogInformation("특정 코스의 월별 라운딩 카운트: 페이지 값이 정수가 아님");
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
                    _logger.LogInformation("특정 코스의 월별 라운딩 카운트: 페이지 사이즈가 범위를 벋어남");
                }
                
                return (false, 30042, null);
            }
            
            if (request.YearRangeStart > request.YearRangeEnd)
            {
                using (LogContext.PushProperty("LogProperty", new 
                       {
                           request = JObject.FromObject(request)
                       }))
                {
                    _logger.LogInformation("특정 코스의 월별 라운딩 카운트: 년도 범위 오류");
                }
                
                return (false, 30043, null);
            }
            
            #endregion

            var query = _db.UsersByCourse
                .AsNoTracking();

            if (request.CourseId != null)
            {
                query = query.Where(p => request.CourseId.Contains(p.CourseId));
            }
            
            if (request.YearRangeStart != null && request.YearRangeEnd != null && request.MonthRangeStart != null && request.MonthRangeEnd != null)
            {
                var searchStartUnixTime = (ulong) new DateTimeOffset(request.YearRangeStart.Value, request.MonthRangeStart.Value, 1, 0, 0, 0, TimeSpan.Zero).ToUnixTimeMilliseconds();

                var yearRangeEnd = request.YearRangeEnd.Value;
                var monthRangeEnd = request.MonthRangeEnd.Value;

                if (monthRangeEnd == 12)
                {
                    yearRangeEnd += 1;
                    monthRangeEnd = 1;
                }
                else
                {
                    monthRangeEnd += 1;
                }

                var searchEndUnixTime = (ulong) new DateTimeOffset(yearRangeEnd, monthRangeEnd, 1, 0, 0, 0, TimeSpan.Zero).ToUnixTimeMilliseconds();
                
                query = query.Where(p => p.Updated >= searchStartUnixTime && p.Updated < searchEndUnixTime);
            }

            var courseIdList = await query
                .GroupBy(p => p.CourseId)
                .OrderByDescending(p => p.Key)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(p => p.Key)
                .ToArrayAsync();
            
            var dataList2 = await query
                .Where(p => courseIdList.Contains(p.CourseId))
                .Select(p => new
                {
                    courseId = p.CourseId,
                    year = UnixTimeHandler.UnixTimeToYear(p.Updated),
                    month = UnixTimeHandler.UnixTimeToMonth(p.Updated)
                })
                .ToListAsync();

            var stopwatch1 = new Stopwatch();
            stopwatch1.Start();
            
            var dataList = await query
                .Select(p => new
                {
                    courseId = p.CourseId,
                    year = UnixTimeHandler.UnixTimeToYear(p.Updated),
                    month = UnixTimeHandler.UnixTimeToMonth(p.Updated)
                })
                .ToListAsync();
            
            stopwatch1.Stop();
            
            _logger.LogInformation($"Database Query 처리 시간: {stopwatch1.ElapsedMilliseconds}ms");

            var courseGroupList = dataList
                .GroupBy(p => p.courseId)
                .Select(p => new
                {
                    courseId = p.Key,
                    countList = p.GroupBy(s => new { s.year, s.month })
                        .Select(s => new
                        {
                            year = s.Key.year,
                            month = s.Key.month,
                            count = s.Count()
                        })
                })
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            var response = new GetCourseRoundingCountByMonthNewResponse();
            response.RoundingCountList = new List<GetCourseRoundingCountByMonthNewListItem>();
                
            var stopwatch2 = new Stopwatch();
            stopwatch2.Start();
            
            foreach (var item in courseGroupList)
            {
                var courseItem = new GetCourseRoundingCountByMonthNewListItem();
                courseItem.CourseId = item.courseId;
                
                var yearItem1 = new JObject();
                var yearItem2 = new Dictionary<int, Dictionary<int, int>>();
                
                int startYear = request.YearRangeStart ?? 2018;
                int endYear = request.YearRangeEnd ?? DateTime.UtcNow.Year;
            
                for (int year = startYear; year <= endYear; year++)
                {
                    var monthItem1 = new JObject();
                    var monthItem2 = new Dictionary<int, int>();
                    
                    int startMonth = year == startYear ? request.MonthRangeStart.Value : 1;
                    int endMonth = year == endYear ? request.MonthRangeEnd.Value : 12;

                    for (int month = startMonth; month <= endMonth; month++)
                    {
                        var count = item.countList.FirstOrDefault(p => p.year == year && p.month == month)?.count;
                        
                        monthItem1.Add(month.ToString(), count);
                        monthItem2.Add(month, count.Value);
                    }
                    
                    yearItem1.Add(year.ToString(), monthItem1);
                    yearItem2.Add(year, monthItem2);
                }

                courseItem.Count1 = yearItem1;
                courseItem.Count2 = yearItem2;
                
                response.RoundingCountList.Add(courseItem);
            }
            
            stopwatch2.Stop();
            
            _logger.LogInformation($"데이터 가공 처리 시간: {stopwatch2.ElapsedMilliseconds}ms");
            ;
            
            var realResponse = new GetCourseRoundingCountByMonthResponse
            {
                // RoundingCountList = dataPageList
            };
            
            return (true, 0, realResponse);
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
                    _logger.LogInformation("코스 별 최고 스코어 데이터: 페이지 값이 정수가 아님");
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
                    _logger.LogInformation("코스 별 최고 스코어 데이터: 페이지 사이즈가 범위를 벋어남");
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
                _logger.LogError(ex, "코스 별 최고 스코어 데이터 조회 중 오류 발생");
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
                    _logger.LogInformation("코스 별 최고 롱기스트 거리 데이터: 페이지 값이 정수가 아님");
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
                    _logger.LogInformation("코스 별 최고 롱기스트 거리 데이터: 페이지 사이즈가 범위를 벋어남");
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
                _logger.LogError(ex, "코스 별 최고 롱기스트 거리 데이터 조회 중 오류 발생");
            }

            return (false, 3006, null);
        }
    }
}