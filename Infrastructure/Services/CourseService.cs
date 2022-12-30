using Infrastructure.Context;
using Infrastructure.Models.Course;
using Infrastructure.Models.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Serilog.Context;

namespace Infrastructure.Services;

public interface ICourseService
{
    Task<(bool isSuccess, int errorCode, GetLongestListByCourseResponse response)> GetLongestListByCourse(GetLongestListByCourseRequest request);
    Task<(bool isSuccess, int errorCode, GetScoreListByCourseResponse response)> GetScoreListByCourse(GetScoreListByCourseRequest request);
}

public class CourseService : ICourseService
{
    // Log 
    private readonly ILogger<CourseService> _logger;
        
    // DB
    private readonly SystemDBContext _db;
        
    // Service
    public CourseService(ILogger<CourseService> logger, SystemDBContext db)
    {
        _logger = logger;

        _db = db;
    }

    public async Task<(bool isSuccess, int errorCode, GetLongestListByCourseResponse response)> GetLongestListByCourse(GetLongestListByCourseRequest request)
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
                    _logger.LogInformation("코스 별 롱기스트 조회: 페이지 값이 정수가 아님");
                }

                return (false, 20001, null);
            }

            if (request.PageSize is not (10 or 20 or 50))
            {
                using (LogContext.PushProperty("LogProperty", new
                       {
                           request = JObject.FromObject(request)
                       }))
                {
                    _logger.LogInformation("코스 별 롱기스트 조회: 페이지 사이즈가 범위를 벋어남");
                }

                return (false, 20002, null);
            }

            var query = _db.UsersByCourse
                .AsNoTracking();

            if (request.CourseId != null)
            {
                query = query.Where(p => p.CourseId == request.CourseId);
            }

            if (request.LongestRangeStart != null && request.LongestRangeEnd != null)
            {
                query = query.Where(p =>
                    p.Longest >= request.LongestRangeStart && p.Longest <= request.LongestRangeEnd);
            }

            // TODO: [20221222-코드리뷰-41번] 노션에 정의해드린 Response Data Example과 동일하게 Response를 변경해주세요. - done
            
            var dataPageList = await query
                .OrderByDescending(p => p.UserByCourseId)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(p => new GetLongestListItem
                {
                    RoundId = p.UserByCourseId,
                    CourseId = p.CourseId,
                    Longest = p.Longest
                }).ToListAsync();
            
            var response = new GetLongestListByCourseResponse
            {
                List = dataPageList
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
                _logger.LogError(ex, "코스 별 롱기스트 조회 중 오류 발생");
            }

            return (false, 2000, null);
        }
    }

    public async Task<(bool isSuccess, int errorCode, GetScoreListByCourseResponse response)> GetScoreListByCourse(GetScoreListByCourseRequest request)
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
                    _logger.LogInformation("코스 별 스코어 조회: 페이지 값이 정수가 아님");
                }

                return (false, 20011, null);
            }

            if (request.PageSize is not (10 or 20 or 50))
            {
                using (LogContext.PushProperty("LogProperty", new
                       {
                           request = JObject.FromObject(request)
                       }))
                {
                    _logger.LogInformation("코스 별 스코어 조회: 페이지 사이즈가 범위를 벋어남");
                }

                return (false, 20012, null);
            }

            var query = _db.UsersByCourse
                .AsNoTracking();

            if (request.CourseId != null)
            {
                query = query.Where(p => p.CourseId == request.CourseId);
            }

            if (request.ScoreRangeStart != null && request.ScoreRangeEnd != null)
            {
                query = query.Where(p =>
                    p.Score >= request.ScoreRangeStart && p.Score <= request.ScoreRangeEnd);
            }

            // TODO: [20221222-코드리뷰-42번] 노션에 정의해드린 Response Data Example과 동일하게 Response를 변경해주세요. - done
            
            var dataPageList = await query
                .OrderByDescending(p => p.UserByCourseId)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(p => new GetScoreListItem
                {
                    RoundId = p.UserByCourseId,
                    CourseId = p.CourseId,
                    Score = p.Score
                }).ToListAsync();
            
            var response = new GetScoreListByCourseResponse
            {
                List = dataPageList
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
                _logger.LogError(ex, "코스 별 스코어 조회 중 오류 발생");
            }

            return (false, 2001, null);
        }
    }
}
