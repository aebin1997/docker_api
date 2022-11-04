using System.Text.RegularExpressions;
using Domain.Entities;
using Infrastructure.Models.Response;
using Infrastructure.Context;
using Infrastructure.Models.Request;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace Infrastructure.Services
{
    public interface IUserService
    {
        Task<(bool isSuccess, int errorCode, UserListResponse response)> GetUsers(GetUsers request);
        Task<(bool isSuccess, int errorCode, UserDetailsResponse details)> GetUserDetails(int idx);
        Task<(bool isSuccess, int errorCode)> AddUser(AddUserRequest request);
        Task<(bool isSuccess, int errorCode)> UpdateUser(UpdateUserParameterModel request);
        Task<(bool isSuccess, int errorCode)> DeleteUser(int idx);
    }
    
    public class UserService : IUserService
    {
        // Log 
        private readonly ILogger<UserService> _logger;
        
        // DB
        private readonly SystemDBContext _db;
        
        // Service

        public UserService(ILogger<UserService> logger, SystemDBContext db)
        {
            _logger = logger;

            _db = db;
        }

        public DateTime DateTimeConverter(DateTime utcDt)
        {
            DateTime localDt;

            localDt = DateTime.SpecifyKind(utcDt, DateTimeKind.Local);
            return localDt;
        }

        public async Task<(bool isSuccess, int errorCode, UserListResponse response)> GetUsers(GetUsers request)
        {
            try
            {
                // TODO: 필터에 대한 유효성 검사 로직 개발
                if (request.Page < 0) 
                {
                    _logger.LogInformation("값이 정수가 아님");
                    
                    return (false, 1004, null);
                }

                // TODO: 필터 로직 개발
                var query = _db.Users
                    .AsNoTracking()
                    .Where(p => p.Deleted == false && p.LifeBestScore >= request.StartLifeBestScore && p.LifeBestScore <= request.EndLifeBestScore);
                    
                var userList = await query
                    .Select(p => new
                    {
                        p.Idx, p.UserId, p.UserPw, p.LifeBestScore, p.Created, p.Updated, p.Deleted
                    })
                    .ToListAsync();
                
                var pageList = userList.OrderByDescending(p => p.Idx)
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToList();

                var response = new UserListResponse();
                response.TotalCount = userList.Count;
                response.List = (from user in pageList
                    select new UserListItem
                    {
                        Idx = user.Idx,
                        UserId = user.UserId,
                        LifeBestScore = user.LifeBestScore,
                        Created = DateTimeConverter(user.Created),
                        Updated = DateTimeConverter(user.Updated),
                        Deleted = user.Deleted
                    }).ToList();

                return (true, 0, response);
            }
            catch (Exception ex)
            {
                using (LogContext.PushProperty("JsonData", new 
                       {
                           page = request.Page,
                           pageSize = request.PageSize
                       }))
                {
                    _logger.LogError(ex, "회원 목록 조회 중 오류 발생");
                }
                
                return (false, 500, null);
            }
        }
        
        public async Task<(bool isSuccess, int errorCode, UserDetailsResponse details)> GetUserDetails(int idx)
        {
            try
            {
                if (idx < 0) 
                {
                    _logger.LogInformation("값이 정수가 아님");
                    
                    return (false, 1004, null);
                }

                var data = await _db.Users
                    .AsNoTracking() 
                    .Where(p => p.Deleted == false && p.Idx == idx)
                    .Select(p => new
                    {
                        p.Idx,
                        p.UserId,
                        p.UserPw,
                        p.LifeBestScore,
                        p.Created,
                        p.Updated,
                        p.Deleted
                    })
                    .FirstOrDefaultAsync();

                if (data == null)
                {
                    return (false, 1005, null);
                }

                var localCreated = DateTimeConverter(data.Created);
                var localUpdated = DateTimeConverter(data.Updated);

                var details = new UserDetailsResponse();
                details.Idx = data.Idx;
                details.UserId = data.UserId;
                details.UserPw = data.UserPw;
                details.LifeBestScore = data.LifeBestScore;
                details.Created = localCreated;
                details.Updated = localUpdated;
                details.Deleted = data.Deleted;

                return (true, 0, details);
            }
            catch (Exception ex)
            {
                using (LogContext.PushProperty("JsonData", new 
                       {
                           idx = idx
                       }))
                {
                    _logger.LogError(ex, "회원 상세 조회 중 오류 발생");
                }
                
                return (false, 500, null);
            }
        }
        
        public async Task<(bool isSuccess, int errorCode)> AddUser(AddUserRequest request)
        {
            try
            {
                // TODO: 유효성 검사 로직 실행
                #region UserId 유효성 검사
                Regex regex = new Regex(@"^[\w_-]+$");

                if (regex.IsMatch(request.UserId) == false)
                {
                    _logger.LogInformation("회원 ID 유효성 검사 실패로 회원 등록 중지");
                    
                    // TODO: User ID에 대한 유효성 검사 실패 로그
                    return (false, 1000);
                }
                #endregion
                
                #region UserPw 유효성 검사
                if (request.UserPw.Length != 4)
                {
                    _logger.LogInformation("회원 Password 유효성 검사 실패로 회원 등록 중지");
                    
                    // TODO: User Password에 대한 유효성 검사 실패 로그
                    return (false, 1001);
                }
                #endregion
                
                var nowUnixTime = DateTime.UtcNow;
                var user = new UserModel
                {
                    UserId = request.UserId,
                    UserPw = request.UserPw,
                    LifeBestScore = request.LifeBestScore,
                    Created = nowUnixTime, 
                    Updated = nowUnixTime, // 없어도 되게 DB 구성했는데 왜 안되는지
                    Deleted = false,
                };

                await _db.Users.AddAsync(user);
                await _db.SaveChangesAsync();

                return (true, 0); 
            }
            catch (Exception ex)
            {
                using (LogContext.PushProperty("JsonData", new 
                       {
                           userId = request.UserId,
                           userPw = request.UserPw,
                           lifeBestScore = request.LifeBestScore
                       }))
                {
                    _logger.LogError(ex, "회원 추가 중 오류 발생");
                }

                return (false, 500);
            }
        }

        public async Task<(bool isSuccess, int errorCode)> UpdateUser(UpdateUserParameterModel request)
        {
            try
            {
                if (request.Idx < 0) 
                {
                    _logger.LogInformation("값이 정수가 아님");
                    
                    return (false, 1004);
                }
                
                // TODO: 유효성 검사 로직 실행
                #region UserId 유효성 검사
                Regex regex = new Regex(@"^[\w_-]+$");

                if (regex.IsMatch(request.UserId) == false)
                {
                    _logger.LogInformation("회원 ID 유효성 검사 실패로 회원 등록 중지");
                    
                    // TODO: User ID에 대한 유효성 검사 실패 로그
                    return (false, 1000);
                }
                #endregion
                
                #region UserPw 유효성 검사
                if (request.UserPw.Length != 4)
                {
                    _logger.LogInformation("회원 Password 유효성 검사 실패로 회원 등록 중지");
                    
                    // TODO: User Password에 대한 유효성 검사 실패 로그
                    return (false, 1001);
                }
                #endregion
                
                var data = await _db.Users
                    .Where(p => p.Deleted == false
                                && p.Idx == request.Idx
                                )
                    .FirstOrDefaultAsync();

                if (data == null)
                {
                    return (false, 1005);
                }

                var updateUserRequest = new List<UpdateUserRequest>();

                if (string.IsNullOrEmpty(request.UserId) == false)
                {
                    updateUserRequest.Add(new UpdateUserRequest()
                    {
                        ColumnName = "user_id",
                        DataBefore = data.UserId,
                        DataAfter = request.UserId
                    });
                    data.UserId = request.UserId;
                }
                
                if (string.IsNullOrEmpty(request.UserPw) == false)
                {
                    updateUserRequest.Add(new UpdateUserRequest()
                    {
                        ColumnName = "user_pw",
                        DataBefore = data.UserPw,
                        DataAfter = request.UserPw
                    });
                    data.UserPw = request.UserPw;
                }
                
                if (request.LifeBestScore > 0)
                {
                    updateUserRequest.Add(new UpdateUserRequest()
                    {
                        ColumnName = "life_best_score",
                        DataBefore = data.LifeBestScore.ToString(),
                        DataAfter = request.LifeBestScore.ToString() 
                    });
                    data.LifeBestScore = (int)request.LifeBestScore;
                }

                if (updateUserRequest.Count > 0)
                {
                    await _db.SaveChangesAsync();
                }
                return (true, 0);
            }
            catch (Exception ex)
            {
                using (LogContext.PushProperty("JsonData", new 
                       {
                           idx = request.Idx,
                           userId = request.UserId,
                           userPw = request.UserPw,
                           lifeBestScore = request.LifeBestScore
                       }))
                {
                    _logger.LogError(ex, "회원 수정 조회 중 오류 발생");
                }
                
                return (false, 500);
            }
        }
        
        public async Task<(bool isSuccess, int errorCode)> DeleteUser(int idx)
        {
            try
            {
                if (idx < 0) 
                {
                    _logger.LogInformation("값이 정수가 아님");
                    
                    return (false, 1004);
                }
                
                var data = await _db.Users
                    .Where(p => p.Deleted == false
                                && p.Idx == idx
                    )
                    .FirstOrDefaultAsync();

                if (data == null)
                {
                    return (false, 1005);
                }

                data.Deleted = true;

                await _db.SaveChangesAsync();
                return (true, 0);
            }
            catch (Exception ex)
            {
                using (LogContext.PushProperty("JsonData", new 
                       {
                           idx = idx
                       }))
                {
                    _logger.LogError(ex, "디비 회원 삭제 중 오류 발생");
                }
                
                return (false, 500);
            }
        }
    }
}