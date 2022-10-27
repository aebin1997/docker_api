using Domain.Entities;
using Infrastructure.Models.Response;
using Infrastructure.Context;
using Infrastructure.Models.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Serilog.Context;

namespace Infrastructure.Services
{
    public interface IUserService
    {
        Task<(bool isSuccess, int errorCode)> AddUser(string userId, string userPw, int? lifeBestScore);
        Task<(bool isSuccess, int errorCode)> DeleteUser(int idx);
        Task<(bool isSuccess, int errorCode, List<UserList> list, int totalCount)> GetUsers();
        Task<(bool isSuccess, int errorCode, UserDetailsResponse details)> GetUserDetails(int idx);
        Task<(bool isSuccess, int errorCode)> UpdateUser(int idx, string userId, string userPw, int? lifeBestScore);
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

        public async Task<(bool isSuccess, int errorCode)> AddUser(string userId, string userPw, int? lifeBestScore)
        {
            try
            {
                var nowUnixTime = DateTime.UtcNow;
                var user = new UserModel
                {
                    Created = nowUnixTime,
                    Deleted = false,
                    LifeBestScore = lifeBestScore,
                    Updated = nowUnixTime,
                    UserId = userId,
                    UserPw = userPw
                };

                await _db.Users.AddAsync(user);
                await _db.SaveChangesAsync();
                
                return (true, 0);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return (false, 3031);
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

        public async Task<(bool isSuccess, int errorCode, List<UserList> list, int totalCount)> GetUsers()
        {
            try
            {
                var query = _db.Users
                    .AsNoTracking() 
                    .Where(p => p.Deleted == false);

                var userList = await query
                    .Select(p => new
                    {
                        p.Idx, p.UserId, p.UserPw, p.LifeBestScore, p.Created, p.Updated, p.Deleted
                    })
                    .ToListAsync();

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

        public async Task<(bool isSuccess, int errorCode)> UpdateUser(int idx, string userId, string userPw, int? lifeBestScore)
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
                    return (false, 3045);
                }

                var updateUserRequest = new List<UpdateUserRequest>();

                if (string.IsNullOrEmpty(userId) == false)
                {
                    updateUserRequest.Add(new UpdateUserRequest()
                    {
                        ColumnName = "user_id",
                        DataBefore = data.UserId,
                        DataAfter = userId
                    });
                    data.UserId = userId;
                }
                
                if (string.IsNullOrEmpty(userPw) == false)
                {
                    updateUserRequest.Add(new UpdateUserRequest()
                    {
                        ColumnName = "user_pw",
                        DataBefore = data.UserPw,
                        DataAfter = userPw
                    });
                    data.UserPw = userPw;
                }
                
                if (lifeBestScore > 0)
                {
                    updateUserRequest.Add(new UpdateUserRequest()
                    {
                        ColumnName = "life_best_score",
                        DataBefore = data.LifeBestScore.ToString(),
                        DataAfter = lifeBestScore.ToString() 
                    });
                    data.LifeBestScore = (int)lifeBestScore;
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
    }
}