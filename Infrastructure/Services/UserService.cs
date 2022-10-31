using Domain.Entities;
using Infrastructure.Models.Response;
using Infrastructure.Context;
using Infrastructure.Models;
using System.Text.RegularExpressions;
using Infrastructure.Models.Request;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services
{
    public interface IUserService
    {
        Task<(bool isSuccess, int errorCode, List<UserList> list, int totalCount)> GetUsers();
        // Task<(bool isSuccess, int errorCode, List<UserList> list, int totalCount)> GetUsersAbove();
        // Task<(bool isSuccess, int errorCode, List<UserList> list, int totalCount)> GetUsersBelow();
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

        public async Task<(bool isSuccess, int errorCode, List<UserList> list, int totalCount)> GetUsers()
        {
            try
            {
                // TODO: 필터에 대한 유효성 검사 로직 개발
                
                var query = _db.Users
                    .AsNoTracking()
                    .Where(p => p.Deleted == false);
                
                // TODO: 필터 로직 개발
                
                var userList = await query
                    .Select(p => new
                    {
                        p.Idx, p.UserId, p.UserPw, p.LifeBestScore, p.Created, p.Updated, p.Deleted
                    })
                    .ToListAsync();

                // TODO: Service Response Model 하나로 반환할 데이터를 다 입력하신 다음 Model 객체 하나만 반환하도록 수정해주세요.
                var totalCount = userList.Count;
                
                var list = (from user in userList
                    select new UserList
                    {
                        Idx = user.Idx,
                        UserId = user.UserId,
                        UserPw = user.UserPw,
                        LifeBestScore = user.LifeBestScore,
                        Created = DateTimeConverter(user.Created),
                        Updated = DateTimeConverter(user.Updated),
                        Deleted = user.Deleted
                    }).ToList();

                return (true, 0, list, totalCount);
            }
            catch
            {
                return (false, 3033, null, 0);
            }
        }
        
        public async Task<(bool isSuccess, int errorCode, UserDetailsResponse details)> GetUserDetails(int idx)
        {
            try
            {
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
                    return (false, 3044, null);
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
            catch
            {
                return (false, 3034, null);
            }
        }
        
        public async Task<(bool isSuccess, int errorCode)> AddUser(AddUserRequest request)
        {
            try
            {
                // TODO: 유효성 검사 로직 실행
                var idValid = false;
                var pwValid = false;
                
                #region UserId 유효성 검사
                Regex regex = new Regex(@"^[\w_-]+$");

                if (regex.IsMatch(request.UserId))
                {
                    idValid = true;
                }
                #endregion
                
                #region UserPw 유효성 검사
                if (request.UserPw.Length == 4)
                {
                    pwValid = true;
                }
                #endregion

                if (idValid == true && pwValid == true)
                {
                    var nowUnixTime = DateTime.UtcNow;
                    var user = new UserModel
                    {
                        UserId = request.UserId,
                        UserPw = request.UserPw,
                        LifeBestScore = request.LifeBestScore,
                        Created = nowUnixTime,
                        Updated = nowUnixTime,
                        Deleted = false,
                    };

                    await _db.Users.AddAsync(user);
                    await _db.SaveChangesAsync();
                    
                    return (true, 0); 
                }
                
                return (false, 3031);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                
                // 에러코드 재정의 해주세요.
                return (false, 3031);
            }
        }

        public async Task<(bool isSuccess, int errorCode)> UpdateUser(UpdateUserParameterModel request)
        {
            try
            {
                var data = await _db.Users
                    .Where(p => p.Deleted == false
                                && p.Idx == request.Idx
                                )
                    .FirstOrDefaultAsync();

                if (data == null)
                {
                    return (false, 3045);
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
            catch (Exception e)
            {
                Console.WriteLine(e);
                return (false, 3035);
            }
        }
        
        public async Task<(bool isSuccess, int errorCode)> DeleteUser(int idx)
        {
            try
            {
                var data = await _db.Users
                    .Where(p => p.Deleted == false
                                && p.Idx == idx
                    )
                    .FirstOrDefaultAsync();

                if (data == null)
                {
                    return (false, 3043);
                }

                data.Deleted = true;

                await _db.SaveChangesAsync();
                
                return (true, 0);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return (false, 3032);
            }
        }
    }
}