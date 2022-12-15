using System.Text.RegularExpressions;
using Domain.Entities;
using Infrastructure.Models.User;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Serilog.Context;

namespace Infrastructure.Services;

public interface IUserService
{
    Task<(bool isSuccess, int errorCode, GetUserBestRecordListResponse response)> GetUserBestRecordList();
    Task<(bool isSuccess, int errorCode, GetUserCourseHistoryListResponse response)> GetUserCourseHistoryList(int page, int pageSize);
    Task<(bool isSuccess, int errorCode, GetUserClubInfoListResponse response)> GetUserClubInfoList();
}
    
public class UserService : IUserService
{
    // Log 
    private readonly ILogger<UserService> _logger;
        
    // DB
    private readonly SystemDBContext _testDB;
        
    // Service
    public UserService(ILogger<UserService> logger, SystemDBContext testDB)
    {
        _logger = logger;

        _testDB = testDB;
    }
    
    public async Task<(bool isSuccess, int errorCode, GetUserBestRecordListResponse response)> GetUserBestRecordList()
    {
        try
        {
            // var dataList2 = await (from user in _testDB.Users
            //     join best in _testDB.UsersBestRecord
            //         on user.UserId equals best.UserId
            //     select new
            //     {
            //         Name = user.Name,
            //         Score = best.Score,
            //         Longest = best.Longest
            //     }).ToListAsync();

            var dataList = await _testDB.Users.Join(
                _testDB.UsersBestRecord,
                user => user.UserId,
                best => best.UserId,
                (user, best) => new
                {
                    Name = user.Name,
                    Score = best.Score,
                    Longest = best.Longest
                }).ToListAsync();

            var response = new GetUserBestRecordListResponse();
            response.List = (from data in dataList
                select new UserBestRecordListItem
                {
                    Name = data.Name,
                    Score = data.Score,
                    Longest = data.Longest
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

    public async Task<(bool isSuccess, int errorCode, GetUserCourseHistoryListResponse response)> GetUserCourseHistoryList(int page, int pageSize)
    {
        try
        {
            var dataList = await _testDB.UsersByCourse
                .AsNoTracking()
                .GroupBy(p => p.UserId)
                .Select(p => new
                {
                    UserId = p.Key,
                    CourseGroup = p.Select(s => new
                    {
                        s.CourseId,
                        s.Score,
                        s.Longest
                    })
                }).ToListAsync();
            
            var dataPageList = dataList.OrderByDescending(p => p.UserId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var response = new GetUserCourseHistoryListResponse();
            
            response.List = (from data in dataPageList
                select new UserCourseHistoryListItem
                {
                    UserId = data.UserId,
                    List = (from course in data.CourseGroup
                            select new UserCourseHistoryItem
                            {
                                CourseId = course.CourseId,
                                Score = course.Score,
                                Longest = course.Longest
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

    public async Task<(bool isSuccess, int errorCode, GetUserClubInfoListResponse response)> GetUserClubInfoList()
    {
        try
        {
            var dataList = await _testDB.UsersByClub
                .AsNoTracking()
                .GroupBy(p => p.UserId)
                .Select(p => new
                {
                    UserId = p.Key,
                    ClubGroup = p.Select(s => new
                    {
                        s.Club,
                        s.Distance
                    })
                }).ToListAsync();
            
            var response = new GetUserClubInfoListResponse();
            
            response.List = (from data in dataList
                select new UserClubInfoListItem
                {
                    UserId = data.UserId,
                    List = (from club in data.ClubGroup
                        select new UserClubInfoItem
                        {
                            Club = club.Club,
                            Distance = club.Distance
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