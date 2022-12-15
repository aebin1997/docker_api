using Infrastructure.Context;
using Infrastructure.Models.Course;
using Infrastructure.Models.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace Infrastructure.Services;

public interface ICourseService
{
    Task<(bool isSuccess, int errorCode, GetLongestListByCourseResponse response)> GetLongestListByCourse(int page, int pageSize, int courseId);
    Task<(bool isSuccess, int errorCode, GetScoreListByCourseResponse response)> GetScoreListByCourse(int page, int pageSize, int courseId);
}

public class CourseService : ICourseService
{
    // Log 
    private readonly ILogger<CourseService> _logger;
        
    // DB
    private readonly SystemDBContext _testDB;
        
    // Service
    public CourseService(ILogger<CourseService> logger, SystemDBContext testDB)
    {
        _logger = logger;

        _testDB = testDB;
    }

    public async Task<(bool isSuccess, int errorCode, GetLongestListByCourseResponse response)> GetLongestListByCourse(int page, int pageSize, int courseId)
    {
        try
        {
            var dataList = await _testDB.UsersByCourse
                .AsNoTracking()
                .GroupBy(p => p.CourseId)
                .Where(p => p.Key == 1)
                .Select(p => new
                {
                    CourseId = p.Key,
                    LongestGroup = p.Select(s => new
                    {
                        s.Longest
                    })
                }).ToListAsync();
            
            var dataPageList = dataList.OrderByDescending(p => p.CourseId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            
            var response = new GetLongestListByCourseResponse();
            
            response.List = (from data in dataPageList
                select new GetLongestListItem()
                {
                    CourseId = data.CourseId,
                    List = (from longest  in data.LongestGroup
                        select new GetLongestItem
                        {
                            Longest = longest.Longest
                        }).ToList()
                }).ToList();
            
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

    public async Task<(bool isSuccess, int errorCode, GetScoreListByCourseResponse response)> GetScoreListByCourse(int page, int pageSize, int courseId)
    {
        try
        {
            var dataList = await _testDB.UsersByCourse
                .AsNoTracking()
                .GroupBy(p => p.CourseId)
                .Where(p => p.Key == 1)
                .Select(p => new
                {
                    CourseId = p.Key,
                    ScoreGroup = p.Select(s => new
                    {
                        s.Score
                    })
                }).ToListAsync();
            
            var dataPageList = dataList.OrderByDescending(p => p.CourseId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            
            var response = new GetScoreListByCourseResponse();
            
            response.List = (from data in dataPageList
                select new GetScoreListItem()
                {
                    CourseId = data.CourseId,
                    List = (from score  in data.ScoreGroup
                        select new GetScoreItem
                        {
                            Score = score.Score,
                        }).ToList()
                }).ToList();
            
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
