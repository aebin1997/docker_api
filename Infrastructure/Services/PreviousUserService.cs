// using System.Text.RegularExpressions;
// using Domain.Entities;
// using Infrastructure.Models.User;
// using Infrastructure.Context;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.Logging;
// using Newtonsoft.Json.Linq;
// using Serilog.Context;
//
// namespace Infrastructure.Services;
//
// public interface IUserService
// {
//     Task<(bool isSuccess, int errorCode, UserListResponse response)> GetUsers(GetUsersRequest request);
//     Task<(bool isSuccess, int errorCode, UserDetailsResponse details)> GetUserDetails(int id);
//     Task<(bool isSuccess, int errorCode)> AddUser(AddUserRequest request);
//     Task<(bool isSuccess, int errorCode)> UpdateUser(UpdateUserParameterRequest request);
//     Task<(bool isSuccess, int errorCode)> DeleteUser(int id);
// }
//     
// public class UserService : IUserService
// {
//     // Log 
//     private readonly ILogger<UserService> _logger;
//         
//     // DB
//     private readonly SystemDBContext _db;
//         
//     // Service
//     public UserService(ILogger<UserService> logger, SystemDBContext db)
//     {
//         _logger = logger;
//
//         _db = db;
//     }
//
//     public DateTime DateTimeConverter(DateTime utcDt)
//     {
//         DateTime localDt;
//
//         localDt = DateTime.SpecifyKind(utcDt, DateTimeKind.Local);
//         return localDt;
//     }
//
//     public async Task<(bool isSuccess, int errorCode, UserListResponse response)> GetUsers(GetUsersRequest request)
//     {
//         try
//         {
//             if (request.Page <= 0) 
//             {
//                 using (LogContext.PushProperty("LogProperty", new 
//                        {
//                            request = JObject.FromObject(request)
//                        }))
//                 {
//                     _logger.LogInformation("회원 목록조회: 페이지 값이 정수가 아님");
//                 }
//                     
//                 return (false, 1000, null);
//             }
//                 
//             // var query = _db.Users
//             //     .AsNoTracking()
//             //     .Where(p => p.Deleted == false && p.LifeBestScore >= request.StartLifeBestScore && p.LifeBestScore <= request.EndLifeBestScore);
//                 
//             var query = _db.Users
//                 .AsNoTracking()
//                 .Where(p => p.Deleted == false);
//                     
//             var userList = await query
//                 .Select(p => new
//                 {
//                     p.UserId, p.Username, p.Password, p.Created, p.Updated, p.Deleted
//                 })
//                 .ToListAsync();
//
//             var pageList = userList.OrderByDescending(p => p.UserId)
//                 .Skip((request.Page - 1) * request.PageSize)
//                 .Take(request.PageSize)
//                 .ToList();
//             
//             // DateTimeConverter(user.Updated),
//             var nowUnixTime = (ulong)DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(); 
//             
//             var response = new UserListResponse();
//             response.TotalCount = userList.Count;
//             response.List = (from user in pageList
//                 select new UserListItem
//                 {
//                     UserId = user.UserId,
//                     Username = user.Username,
//                     Created = nowUnixTime,
//                     Updated = nowUnixTime,
//                     Deleted = user.Deleted
//                 }).ToList();
//
//             return (true, 0, response);
//         }
//         catch (Exception ex)
//         {
//             using (LogContext.PushProperty("LogProperty", new 
//                    {
//                        request = JObject.FromObject(request)
//                    }))
//             {
//                 _logger.LogError(ex, "회원 목록 조회 중 오류 발생");
//             }
//                 
//             return (false, 1001, null);
//         }
//     }
//         
//     public async Task<(bool isSuccess, int errorCode, UserDetailsResponse details)> GetUserDetails(int id)
//     {
//         try
//         {
//             if (id <= 0) 
//             {
//                 using (LogContext.PushProperty("LogProperty", new 
//                        {
//                            userId = id
//                        }))
//                 {
//                     _logger.LogInformation("회원 상세 조회: 값이 정수가 아님");
//                 }
//                     
//                 return (false, 1002, null);
//             }
//             
//             var data = await _db.Users
//                 .AsNoTracking() 
//                 .Where(p => p.Deleted == false && p.UserId == id)
//                 .Select(p => new
//                 {
//                     p.UserId,
//                     p.Username,
//                     p.Password,
//                     p.Name,
//                     p.Created,
//                     p.Updated,
//                     p.Deleted
//                 })
//                 .FirstOrDefaultAsync();
//
//             if (data == null)
//             {
//                 using (LogContext.PushProperty("LogProperty", new 
//                        {
//                            userId = id
//                        }))
//                 {
//                     _logger.LogError("회원 상세조회: 데이터 불러오기 실패");
//                 }                    
//
//                 return (false, 1003, null);
//             }
//                 
//             var nowUnixTime = (ulong)DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(); 
//             
//             var details = new UserDetailsResponse();
//             details.UserId = data.UserId;
//             details.Username = data.Username;
//             details.Password = data.Password;
//             details.Name = data.Name;
//             details.Created = nowUnixTime;
//             details.Updated = nowUnixTime;
//             details.Deleted = data.Deleted;
//
//             return (true, 0, details);
//         }
//         catch (Exception ex)
//         {
//             using (LogContext.PushProperty("LogProperty", new 
//                    {
//                        userId = id
//                    }))
//             {
//                 _logger.LogError(ex, "회원 상세 조회 중 오류 발생");
//             }
//                 
//             return (false, 1004, null);
//         }
//     }
//         
//     public async Task<(bool isSuccess, int errorCode)> AddUser(AddUserRequest request)
//     {
//         try
//         {
//             #region UserId 유효성 검사
//             Regex regex = new Regex(@"^[\w_-]+$");
//
//             if (regex.IsMatch(request.Username) == false)
//             {
//                 using (LogContext.PushProperty("LogProperty", new 
//                        {
//                            userName = request.Username
//                        }))
//                 {
//                     _logger.LogInformation("회원 ID 유효성 검사 실패로 회원 등록 중지");
//                 }
//
//                 return (false, 1005);
//             }
//             #endregion
//                 
//             #region UserPw 유효성 검사
//             if (request.Password.Length != 4)
//             {
//                 using (LogContext.PushProperty("LogProperty", new 
//                        {
//                            userPassword = request.Password
//                        }))
//                 {
//                     _logger.LogInformation("회원 Password 유효성 검사 실패로 회원 등록 중지");
//                 }
//                     
//                 return (false, 1006);
//             }
//             #endregion
//                 
//             var nowUnixTime = (ulong)DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(); 
//             
//             var user = new UserModel
//             {
//                 Username = request.Username,
//                 Password = request.Password,
//                 Name = request.Name,
//                 Created = nowUnixTime, 
//                 Updated = nowUnixTime, 
//                 Deleted = false,
//             };
//
//             await _db.Users.AddAsync(user);
//             await _db.SaveChangesAsync();
//
//             return (true, 0); 
//         }
//         catch (Exception ex)
//         {
//             using (LogContext.PushProperty("LogProperty", new 
//                    {
//                        request = JObject.FromObject(request)
//                    }))
//             {
//                 _logger.LogError(ex, "회원 추가 중 오류 발생");
//             }
//                 
//             return (false, 1007);
//         }
//     }
//
//     public async Task<(bool isSuccess, int errorCode)> UpdateUser(UpdateUserParameterRequest request)
//     {
//         try
//         {
//             if (request.UserId <= 0) 
//             {
//                 using (LogContext.PushProperty("LogProperty", new 
//                        {
//                            userId = request.UserId
//                        }))
//                 {
//                     _logger.LogInformation("회원 정보 수정: 값이 정수가 아님");
//                 }
//                     
//                 return (false, 1008);
//             }
//                 
//             #region UserId 유효성 검사
//             Regex regex = new Regex(@"^[\w_-]+$");
//
//             if (regex.IsMatch(request.Username) == false)
//             {
//                 using (LogContext.PushProperty("LogProperty", new 
//                        {
//                            userId = request.UserId
//                        }))
//                 {
//                     _logger.LogInformation("회원 ID 유효성 검사 실패로 회원 등록 중지");
//                 }
//                     
//                 return (false, 1010);
//             }
//             #endregion
//                 
//             #region UserPw 유효성 검사
//             if (request.Password.Length != 4)
//             {
//                 using (LogContext.PushProperty("LogProperty", new 
//                        {
//                            request = JObject.FromObject(request)
//                        }))
//                 {
//                     _logger.LogInformation("회원 Password 유효성 검사 실패로 회원 등록 중지");
//                 }
//                     
//                 return (false, 1011);
//             }
//             #endregion
//                 
//             var data = await _db.Users
//                 .Where(p => p.Deleted == false
//                             && p.UserId == request.UserId
//                 )
//                 .FirstOrDefaultAsync();
//
//             if (data == null)
//             {
//                 using (LogContext.PushProperty("LogProperty", new 
//                        {
//                            request = JObject.FromObject(request)
//                        }))
//                 {
//                     _logger.LogError("회원 수정을 위한 회원 조회 실패");
//                 }
//                     
//                 return (false, 1009);
//             }
//
//             var updateUserRequest = new List<UpdateUserRequest>();
//
//             if (string.IsNullOrEmpty(request.Username) == false)
//             {
//                 updateUserRequest.Add(new UpdateUserRequest()
//                 {
//                     ColumnName = "username",
//                     DataBefore = data.Username,
//                     DataAfter = request.Username
//                 });
//                 data.UserId = request.UserId;
//             }
//                 
//             if (string.IsNullOrEmpty(request.Password) == false)
//             {
//                 updateUserRequest.Add(new UpdateUserRequest()
//                 {
//                     ColumnName = "user_pw",
//                     DataBefore = data.Password,
//                     DataAfter = request.Password
//                 });
//                 data.Password = request.Password;
//             }
//                 
//             if (updateUserRequest.Count > 0)
//             {
//                 await _db.SaveChangesAsync();
//             }
//                 
//             return (true, 0);
//         }
//         catch (Exception ex)
//         {
//             using (LogContext.PushProperty("LogProperty", new 
//                    {
//                        request = JObject.FromObject(request)
//                    }))
//             {
//                 _logger.LogError(ex, "회원 수정 중 오류 발생");
//             }
//                 
//             return (false, 1012);
//         }
//     }
//         
//     public async Task<(bool isSuccess, int errorCode)> DeleteUser(int id)
//     {
//         try
//         {
//             if (id <= 0) 
//             {
//                 using (LogContext.PushProperty("LogProperty", new 
//                        {
//                            userId = id
//                        }))
//                 {
//                     _logger.LogInformation("회원 삭제: 값이 정수가 아님");
//                 }
//                     
//                 return (false, 1013);
//             }
//                 
//             var data = await _db.Users
//                 .Where(p => p.Deleted == false
//                             && p.UserId == id
//                 )
//                 .FirstOrDefaultAsync();
//
//             if (data == null)
//             {
//                 using (LogContext.PushProperty("LogProperty", new 
//                        {
//                            userId = id
//                        }))
//                 {
//                     _logger.LogInformation("회원 삭제: 삭제할 대상이 없음");
//                 }
//                     
//                 return (false, 1014);
//             }
//
//             data.Deleted = true;
//
//             await _db.SaveChangesAsync();
//             return (true, 0);
//         }
//         catch (Exception ex)
//         {
//             using (LogContext.PushProperty("LogProperty", new 
//                    {
//                        userId = id
//                    }))
//             {
//                 _logger.LogError(ex, "디비 회원 삭제 중 오류 발생");
//             }
//                 
//             return (false, 1015);
//         }
//     }
// }