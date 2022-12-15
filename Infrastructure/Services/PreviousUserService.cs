// using System.Text.RegularExpressions;
// using Domain.Entities;
// using Infrastructure.Models.User;
// using Infrastructure.Context;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.Logging;
// using Newtonsoft.Json.Linq;
// using Serilog.Context;
//
// // TODO: [20221106-권용진] 8번 (property 삭제 완료)
// // TODO: [20221106-권용진] LogContext.PushProperty를 통해 JsonData라는 Property를 로그에 넣는 부분은 Serilog의 기능이며, 현재 설정되어있지 않아 동작하지 않는 코드입니다.
// // TODO: [20221106-권용진] 이 부분은 제대로 이해하지 못한 상태로 사용한 부분으로 판단됩니다.
//
// // TODO: [20221106-권용진] 9번 (해결 완료)
// // TODO: [20221106-권용진] 서비스 메서드에서 반환되는 에러코드가 고유하지 않아 에러 발생시 추적이 어려워지는 상황이 발생하였습니다.
// // TODO: [20221106-권용진] 비즈니스 로직에서 반환되는 에러코드는 절대 중복이 되선 안됩니다.
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
//                 // TODO: [20221106-권용진] 6번 (수정 완료)
//                 // TODO: [20221106-권용진] 에러 코드를 반한할 때는 로그를 무조건 추가해주셔야합니다.
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
//             // TODO: [20221106-권용진] 7번 (수정 완료)
//             // TODO: [20221106-권용진] 별도의 변수를 선언하여 처리하신 이유가 무엇인가요 ? - 그냥 이유는 없었어요..!
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
//             // TODO: 유효성 검사 로직 실행
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
//                 // TODO: User ID에 대한 유효성 검사 실패 로그
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
//                 // TODO: User Password에 대한 유효성 검사 실패 로그
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
//                 // TODO: [20221106-권용진] 12번 (수정 완료)
//                 // TODO: [20221106-권용진] 에러 코드를 반한할 때는 로그를 무조건 추가해주셔야합니다.
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
//             // TODO: [20221106-권용진] 14번 (답변 완료)
//             // TODO: [20221106-권용진] 아래와 같은 로직을 보고 어떤 이유에서 로직을 작성하였는지 짐작하신게 있는지 궁금합니다. - nullable
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