using System.Text.RegularExpressions;
using Domain.Entities;
using Infrastructure.Models.Response;
using Infrastructure.Context;
using Infrastructure.Models.Request;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog.Context;

// TODO: [20221106-권용진] 8번
// TODO: [20221106-권용진] LogContext.PushProperty를 통해 JsonData라는 Property를 로그에 넣는 부분은 Serilog의 기능이며, 현재 설정되어있지 않아 동작하지 않는 코드입니다.
// TODO: [20221106-권용진] 이 부분은 제대로 이해하지 못한 상태로 사용한 부분으로 판단됩니다.

// TODO: [20221106-권용진] 9번
// TODO: [20221106-권용진] 서비스 메서드에서 반환되는 에러코드가 고유하지 않아 에러 발생시 추적이 어려워지는 상황이 발생하였습니다.
// TODO: [20221106-권용진] 비즈니스 로직에서 반환되는 에러코드는 절대 중복이 되선 안됩니다.

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
                if (request.Page < 0) 
                {
                    _logger.LogInformation("값이 정수가 아님");
                    
                    return (false, 1004, null);
                }
                
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
                // TODO: [20221106-권용진] 5번
                // TODO: [20221106-권용진] idx가 0일때도 검색을 진행해야할까요 ?
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
                    // TODO: [20221106-권용진] 6번
                    // TODO: [20221106-권용진] 에러 코드를 반한할 때는 로그를 무조건 추가해주셔야합니다.
                    return (false, 1005, null);
                }
                
                // TODO: [20221106-권용진] 7번
                // TODO: [20221106-권용진] 별도의 변수를 선언하여 처리하신 이유가 무엇인가요 ? 
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
                #region UserId 유효성 검사
                Regex regex = new Regex(@"^[\w_-]+$");

                if (regex.IsMatch(request.UserId) == false)
                {
                    _logger.LogInformation("회원 ID 유효성 검사 실패로 회원 등록 중지");
                    
                    return (false, 1000);
                }
                #endregion
                
                #region UserPw 유효성 검사
                if (request.UserPw.Length != 4)
                {
                    _logger.LogInformation("회원 Password 유효성 검사 실패로 회원 등록 중지");
                    
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
                    // TODO: [박예빈] 없어도 되게 DB 구성했는데 왜 안되는지
                    // TODO: [20221106-권용진] DB Table을 Default값을 설정한 후 EF Core에서 별도로 설정해주지 않으면 DB Table에 설정된 Default값을 사용하지 않습니다. 
                    Updated = nowUnixTime, 
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

        // TODO: [20221106-권용진] 13번
        // TODO: [20221106-권용진] 서비스 메서드에서 받는 model naming 규칙을 지켜주셔야합니다.
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
                    // TODO: [20221106-권용진] 12번
                    // TODO: [20221106-권용진] 에러 코드를 반한할 때는 로그를 무조건 추가해주셔야합니다.
                    return (false, 1005);
                }

                // TODO: [20221106-권용진] 14번 
                // TODO: [20221106-권용진] 아래와 같은 로직을 보고 어떤 이유에서 로직을 작성하였는지 짐작하신게 있는지 궁금합니다.
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