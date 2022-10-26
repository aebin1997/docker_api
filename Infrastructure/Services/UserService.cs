using Domain.Entities;
using Infrastructure.Models.Response;
using Infrastructure.Context;
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
        (bool isSuccess, List<UserList> list, int totalCount) GetUsers();
        (bool isSuccess, UserDetailsResponse details) GetUserDetails(int idx);
        (bool isSuccess, UserDetailsResponse details) UpdateUser(int idx, string userId, string userPw, int lifeBestScore);
    }
    
    public class UserService : IUserService
    {
        // Log 
        private readonly ILogger<UserService> _logger;
        
        // DB
        private readonly TestDBContext _db;
        
        // Service

        public UserService(ILogger<UserService> logger, TestDBContext db)
        {
            _logger = logger;

            _db = db;
        }

        private int AutoIncrement { get; set; } = 0;
        
        public async Task<(bool isSuccess, int errorCode)> AddUser(string userId, string userPw, int? lifeBestScore)
        {
            try
            {
                AutoIncrement += 1;
                var nowUnixTime = (ulong)DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                Console.WriteLine(nowUnixTime);
                var user = new UserModel
                {
                    Idx = AutoIncrement,
                    UserId = userId,
                    UserPw = userPw,
                    LifeBestScore = lifeBestScore,
                    Created = nowUnixTime,
                    Updated = nowUnixTime,
                    Deleted = false
                };

                await _db.User.AddAsync(user);
                await _db.SaveChangesAsync();
                
                return (true, 0);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<(bool isSuccess, int errorCode)> DeleteUser(int idx)
        {
            try
            {
                
                
                return (true, 0);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            // try
            // {
            //     var data = await _db.User
            //         .Where(p => p.Deleted == false 
            //                     && p.Idx = idx
            //         )
            //         .FirstOrDefaultAsync();
            //
            //
            //     // #region delete logic
            //     //
            //     // if (data.Delete != false)
            //     // {
            //     //     
            //     // }
            //     //
            //     //
            //     // #endregion
            // }
        }

        public (bool isSuccess, List<UserList> list, int totalCount) GetUsers()
        {
            throw new NotImplementedException();
        }

        public (bool isSuccess, UserDetailsResponse details) GetUserDetails(int idx)
        {
            throw new NotImplementedException();
        }

        public (bool isSuccess, UserDetailsResponse details) UpdateUser(int idx, string userId, string userPw, int lifeBestScore)
        {
            throw new NotImplementedException();
        }
    }
}