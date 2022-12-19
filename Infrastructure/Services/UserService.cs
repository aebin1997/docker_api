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
    
    // TODO: [20221219-코드리뷰-16번] 기존에 개발하신 회원 관련 로직도 다시 추가해주세요. (가입, 수정, 삭제 등)
    
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
            
            
            // TODO: [20221219-코드리뷰-18번] 노션에 작성되어있는 필터 조건 추가해주세요.
            
            // TODO: [20221219-코드리뷰-17번] select하는 쿼리는 비추적 쿼리로 작성하셔야합니다.
            var dataList = await _testDB.Users.Join(
                _testDB.UsersBestRecord,
                user => user.UserId,
                best => best.UserId,
                (user, best) => new UserBestRecordListItem
                {
                    Name = user.Name,
                    Score = best.Score,
                    Longest = best.Longest
                }).ToListAsync();

            var response = new GetUserBestRecordListResponse();
            response.List = dataList;

            return (true, 0, response);
        }
        catch (Exception ex)
        {
            // TODO: [20221219-코드리뷰-19번] 로그 메시지 수정해주세요.
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
            // TODO: [20221219-코드리뷰-20번] 붎필요한 로직을 제거해주세요.
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
            // TODO: [20221219-코드리뷰-21번] 로그 메시지 수정해주세요.
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
            // TODO: [20221219-코드리뷰-23번] 붎필요한 로직을 제거해주세요.
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
            // TODO: [20221219-코드리뷰-22번] 로그 메시지 수정해주세요.
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