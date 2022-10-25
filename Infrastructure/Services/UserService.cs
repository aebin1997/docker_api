using Infrastructure.Models.Response;
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
        bool AddUser(string userId, string userPw, int lifeBestScore);
        bool DeleteUser(int idx);
        (bool isSuccess, List<UserList> list, int totalCount) GetUsers();
        (bool isSuccess, UserDetailsResponse details) GetUserDetails(int idx);
        (bool isSuccess, UserDetailsResponse details) UpdateUser(int idx, string userId, string userPw, int lifeBestScore);
    }
    
    public class UserService : IUserService
    {
        // Log 
        private readonly ILogger<UserService> _logger;
        
        // DB
        
        // Service

        public UserService(ILogger<UserService> logger)
        {
            _logger = logger;
        }

        private int autoIncrement { get; set; } = 0;
        
        public bool AddUser(string userId, string userPw, int lifeBestScore)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public bool DeleteUser(int idx)
        {
            throw new NotImplementedException();
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