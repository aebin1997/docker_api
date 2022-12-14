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
    Task<(bool isSuccess, int errorCode, GetUserBestRecordListResponse response)> GetUserBestRecordList(GetUserBestRecordListRequest request);
    Task<(bool isSuccess, int errorCode, GetUserCourseHistoryListResponse response)> GetUserCourseHistoryList(GetUserCourseHistoryListRequest request);
    Task<(bool isSuccess, int errorCode, GetUserClubInfoListResponse response)> GetUserClubInfoList(GetUserClubInfoListRequest request);
    Task<(bool isSuccess, int errorCode, UserListResponse response)> GetUsers(GetUsersRequest request);
    Task<(bool isSuccess, int errorCode, UserDetailsResponse details)> GetUserDetails(int id);
    Task<(bool isSuccess, int errorCode)> AddUser(AddUserRequest request);
    Task<(bool isSuccess, int errorCode)> UpdateUser(UpdateUserParameterRequest request);
    Task<(bool isSuccess, int errorCode)> DeleteUser(int id);
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

    public async Task<(bool isSuccess, int errorCode, GetUserBestRecordListResponse response)> GetUserBestRecordList(GetUserBestRecordListRequest request)
    {
        try
        {
            if (request.Page <= 0) 
            {
                using (LogContext.PushProperty("LogProperty", new 
                       {
                           request = JObject.FromObject(request)
                       }))
                {
                    _logger.LogInformation("회원 별 최고 기록: 페이지 값이 정수가 아님");
                }
                
                return (false, 10001, null);
            }
            
            if (request.PageSize is not (10 or 20 or 50)) 
            {
                using (LogContext.PushProperty("LogProperty", new 
                       {
                           request = JObject.FromObject(request)
                       }))
                {
                    _logger.LogInformation("회원 별 최고 기록: 페이지 사이즈가 범위를 벋어남");
                }
                
                return (false, 10002, null);
            }

            var query = _db.Users
                .AsNoTracking()
                .Join(
                    _db.UsersBestRecord.AsNoTracking(),
                    user => user.UserId,
                    best => best.UserId,
                    (user, best) => new 
                    {
                        UserId = user.UserId,
                        Name = user.Name,
                        Score = best.Score,
                        Longest = best.Longest
                    });

            if (request.UserId != null)
            {
                query = query.Where(p => request.UserId.Contains(p.UserId));
            }
            
            if (request.BestRecordType != null && request.BestRecordRangeStart != null && request.BestRecordRangeEnd != null)
            {
                query = request.BestRecordType switch
                {
                    1 => query.Where(p =>
                        p.Score >= request.BestRecordRangeStart && p.Score <= request.BestRecordRangeEnd),
                    2 => query.Where(p =>
                        p.Longest >= request.BestRecordRangeStart && p.Score <= request.BestRecordRangeEnd),
                    _ => query
                };
            }
            
            var dataPageList = await query
                .OrderByDescending(p => p.UserId)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(p => new UserBestRecordListItem
                {
                    UserId = p.UserId,
                    Name = p.Name,
                    Score = p.Score,
                    Longest = p.Longest
                }).ToListAsync();

            var response = new GetUserBestRecordListResponse
            {
                List = dataPageList
            };

            return (true, 0, response);
        }
        catch (Exception ex)
        {
            using (LogContext.PushProperty("LogProperty", new 
                   {
                       request = JObject.FromObject(request)
                   }))
            {
                _logger.LogError(ex, "회원 별 최고기록 조회 중 오류 발생");
            }
            
            return (false, 1000, null);
        }
    }

    public async Task<(bool isSuccess, int errorCode, GetUserCourseHistoryListResponse response)> GetUserCourseHistoryList(GetUserCourseHistoryListRequest request)
    {
        try
        {
            if (request.Page <= 0) 
            {
                using (LogContext.PushProperty("LogProperty", new 
                       {
                           request = JObject.FromObject(request)
                       }))
                {
                    _logger.LogInformation("회원 별 코스 기록: 페이지 값이 정수가 아님");
                }
                    
                return (false, 10021, null);
            }

            var query = _db.UsersByCourse
                .AsNoTracking();
            
            // 단일 조회
            if (request.UserId != null)
            {
                query = query.Where(p => p.UserId == request.UserId);
            }

            // 다중 조회
            if (request.CourseId != null)
            {
                query = query.Where(p => request.CourseId.Contains(p.CourseId));
            }

            var dataPageList = await query
                .GroupBy(p => p.UserId)
                .OrderByDescending(p => p.Key)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(p => new UserCourseHistoryListItem
                {
                    UserId = p.Key,
                    List = p.Select(s => new UserCourseHistoryItem
                    {
                        CourseId = s.CourseId,
                        Score = s.Score,
                        Longest = s.Longest
                    }).ToList()
                }).ToListAsync();

            var response = new GetUserCourseHistoryListResponse
            {
                List = dataPageList
            };

            return (true, 0, response);
        }
        catch (Exception ex)
        {
            using (LogContext.PushProperty("JsonData", new
                   {
                       request = JObject.FromObject(request)
                   }))
            {
                _logger.LogError(ex, "회원 별 코스기록 조회 중 오류 발생");
            }

            return (false, 1002, null);
        }
    }

    public async Task<(bool isSuccess, int errorCode, GetUserClubInfoListResponse response)> GetUserClubInfoList(GetUserClubInfoListRequest request)
    {
        try
        {
            if (request.Page <= 0)
            {
                using (LogContext.PushProperty("LogProperty", new
                       {
                           request = JObject.FromObject(request)
                       }))
                {
                    _logger.LogInformation("회원 별 클럽 기록: 페이지 값이 정수가 아님");
                }

                return (false, 10031, null);
            }

            if (request.PageSize is not (10 or 20 or 50))
            {
                using (LogContext.PushProperty("LogProperty", new
                       {
                           request = JObject.FromObject(request)
                       }))
                {
                    _logger.LogInformation("회원 별 클럽 기록: 페이지 사이즈가 범위를 벋어남");
                }

                return (false, 10032, null);
            }

            var query = _db.UsersByClub
                .AsNoTracking();

            if (request.UserId != null)
            { 
                query = query.Where(p => p.UserId == request.UserId); 
            }
            
            if (request.Club != null)
            {
                query = query.Where(p => request.Club.Contains(p.Club));
            }
            
            var dataPageList = await query
                .GroupBy(p => p.UserId)
                .OrderByDescending(p => p.Key)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(p => new UserClubInfoListItem
                {
                    UserId = p.Key,
                    List = p.Select(s => new UserClubInfoItem
                    {
                        Club = s.Club,
                        Distance = s.Distance
                    }).ToList()
                }).ToListAsync();
            
            var response = new GetUserClubInfoListResponse
            {
                List = dataPageList
            };

            return (true, 0, response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "회원 별 클럽 거리 정보 조회 중 오류 발생");

            return (false, 1003, null);
        }
    }
    
    public async Task<(bool isSuccess, int errorCode, UserListResponse response)> GetUsers(GetUsersRequest request)
    {
        try
        {
            if (request.Page <= 0) 
            {
                using (LogContext.PushProperty("LogProperty", new 
                       {
                           request = JObject.FromObject(request)
                       }))
                {
                    _logger.LogInformation("회원 목록조회: 페이지 값이 정수가 아님");
                }
                    
                return (false, 10041, null);
            }
                
            var query = _db.Users
                .AsNoTracking()
                .Where(p => p.Deleted == false);
                    
            var dataPageList = await query
                .OrderByDescending(p => p.UserId)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(p => new UserListItem
                {
                    UserId = p.UserId,
                    Username = p.Username,
                    Created = UnixTimeHandler.UnixTimeToDateTime(p.Created),
                    Updated = UnixTimeHandler.UnixTimeToDateTime(p.Updated),
                    Deleted = p.Deleted
                })
                .ToListAsync();

            var response = new UserListResponse
            {
                TotalCount = dataPageList.Count,
                List = dataPageList
            };

            return (true, 0, response);
        }
        catch (Exception ex)
        {
            using (LogContext.PushProperty("LogProperty", new 
                   {
                       request = JObject.FromObject(request)
                   }))
            {
                _logger.LogError(ex, "회원 목록 조회 중 오류 발생");
            }
                
            return (false, 1004, null);
        }
    }
        
    public async Task<(bool isSuccess, int errorCode, UserDetailsResponse details)> GetUserDetails(int id)
    {
        try
        {
            if (id <= 0) 
            {
                using (LogContext.PushProperty("LogProperty", new 
                       {
                           userId = id
                       }))
                {
                    _logger.LogInformation("회원 상세 조회: 값이 정수가 아님");
                }
                    
                return (false, 10051, null);
            }
            
            var data = await _db.Users
                .AsNoTracking() 
                .Where(p => p.Deleted == false && p.UserId == id)
                .Select(p => new
                {
                    p.UserId,
                    p.Username,
                    p.Password,
                    p.Name,
                    p.Created,
                    p.Updated,
                    p.Deleted
                })
                .FirstOrDefaultAsync();

            if (data == null)
            {
                using (LogContext.PushProperty("LogProperty", new 
                       {
                           userId = id
                       }))
                {
                    _logger.LogError("회원 상세조회: 데이터 불러오기 실패");
                }                    
                
                return (false, 10052, null);
            }
                
            var details = new UserDetailsResponse();
            details.UserId = data.UserId;
            details.Username = data.Username;
            details.Password = data.Password;
            details.Name = data.Name;
            details.Created = UnixTimeHandler.UnixTimeToDateTime(data.Created);
            details.Updated = UnixTimeHandler.UnixTimeToDateTime(data.Updated);
            details.Deleted = data.Deleted;

            return (true, 0, details);
        }
        catch (Exception ex)
        {
            using (LogContext.PushProperty("LogProperty", new 
                   {
                       userId = id
                   }))
            {
                _logger.LogError(ex, "회원 상세 조회 중 오류 발생");
            }
                
            return (false, 1005, null);
        }
    }
        
    public async Task<(bool isSuccess, int errorCode)> AddUser(AddUserRequest request)
    {
        try
        {
            #region Username 유효성 검사
            Regex regex = new Regex(@"^[\w_-]+$");

            if (regex.IsMatch(request.Username) == false)
            {
                using (LogContext.PushProperty("LogProperty", new 
                       {
                           username = request.Username
                       }))
                {
                    _logger.LogInformation("회원 Username(ID) 유효성 검사 실패로 회원 등록 중지");
                }

                return (false, 10061);
            }
            #endregion
                
            #region Password 유효성 검사
            if (request.Password.Length != 4)
            {
                using (LogContext.PushProperty("LogProperty", new 
                       {
                           userPassword = request.Password
                       }))
                {
                    _logger.LogInformation("회원 Password 유효성 검사 실패로 회원 등록 중지");
                }
                    
                return (false, 10062);
            }
            #endregion
            
            #region Name 유효성 검사

            if (regex.IsMatch(request.Name) == false)
            {
                using (LogContext.PushProperty("LogProperty", new 
                       {
                           name = request.Name
                       }))
                {
                    _logger.LogInformation("회원 Name 유효성 검사 실패로 회원 등록 중지");
                }

                return (false, 10063);
            }
            #endregion
                
            var nowUnixTime = (ulong)DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(); 
            
            var user = new UserModel
            {
                Username = request.Username,
                Password = request.Password,
                Name = request.Name,
                Created = nowUnixTime, 
                Updated = nowUnixTime, 
                Deleted = false,
            };

            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();

            return (true, 0); 
        }
        catch (Exception ex)
        {
            using (LogContext.PushProperty("LogProperty", new 
                   {
                       request = JObject.FromObject(request)
                   }))
            {
                _logger.LogError(ex, "회원 추가 중 오류 발생");
            }
                
            return (false, 1006);
        }
    }

    public async Task<(bool isSuccess, int errorCode)> UpdateUser(UpdateUserParameterRequest request)
    {
        try
        {
            if (request.UserId <= 0)
            {
                using (LogContext.PushProperty("LogProperty", new
                       {
                           request = JObject.FromObject(request)
                       }))
                {
                    _logger.LogInformation("회원 정보 수정: 값이 정수가 아님");
                }

                return (false, 10071);
            }

            #region Username 유효성 검사

            Regex regex = new Regex(@"^[\w_-]+$");

            if (request.Username != null)
            {
                if (regex.IsMatch(request.Username) == false)
                {
                    using (LogContext.PushProperty("LogProperty", new
                           {
                               request = JObject.FromObject(request)
                           }))
                    {
                        _logger.LogInformation("회원 Username 유효성 검사 실패로 회원 등록 중지");
                    }

                    return (false, 10072);
                }
            }

            #endregion

            #region Password 유효성 검사

            if (request.Password != null)
            {
                if (request.Password.Length != 4)
                {
                    using (LogContext.PushProperty("LogProperty", new 
                           {
                               request = JObject.FromObject(request)
                           }))
                    {
                        _logger.LogInformation("회원 Password 유효성 검사 실패로 회원 등록 중지");
                    }
                    
                    return (false, 1011);
                } 
            } 
            
            #endregion
            
            #region Name 유효성 검사
            
            if (request.Name != null)
            {
                if (regex.IsMatch(request.Name) == false)
                {
                    using (LogContext.PushProperty("LogProperty", new
                           {
                               request = JObject.FromObject(request)
                           }))
                    {
                        _logger.LogInformation("회원 Name 유효성 검사 실패로 회원 등록 중지");
                    }

                    return (false, 10073);
                }
            }
            
            #endregion
                
            var data = await _db.Users
                .Where(p => p.Deleted == false
                            && p.UserId == request.UserId
                )
                .FirstOrDefaultAsync();

            if (data == null)
            {
                using (LogContext.PushProperty("LogProperty", new 
                       {
                           request = JObject.FromObject(request)
                       }))
                {
                    _logger.LogError("회원 수정을 위한 회원 조회 실패");
                }
                    
                return (false, 10074);
            }

            var updateUserRequest = new List<UpdateUserRequest>();

            if (string.IsNullOrEmpty(request.Username) == false)
            {
                updateUserRequest.Add(new UpdateUserRequest()
                {
                    ColumnName = "username",
                    DataBefore = data.Username,
                    DataAfter = request.Username
                });
                data.UserId = request.UserId;
            }
                
            if (string.IsNullOrEmpty(request.Password) == false)
            {
                updateUserRequest.Add(new UpdateUserRequest()
                {
                    ColumnName = "password",
                    DataBefore = data.Password,
                    DataAfter = request.Password
                });
                data.Password = request.Password;
            }
            
            if (string.IsNullOrEmpty(request.Name) == false)
            {
                updateUserRequest.Add(new UpdateUserRequest()
                {
                    ColumnName = "name",
                    DataBefore = data.Name,
                    DataAfter = request.Name
                });
                data.Name = request.Name;
            }
            
            var nowUnixTime = (ulong)DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(); 
            
            if (updateUserRequest.Count > 0)
            {
                data.Updated = nowUnixTime;
                await _db.SaveChangesAsync();
            }
                
            return (true, 0);
        }
        catch (Exception ex)
        {
            using (LogContext.PushProperty("LogProperty", new 
                   {
                       request = JObject.FromObject(request)
                   }))
            {
                _logger.LogError(ex, "회원 수정 중 오류 발생");
            }
                
            return (false, 1007);
        }
    }
        
    public async Task<(bool isSuccess, int errorCode)> DeleteUser(int id)
    {
        try
        {
            if (id <= 0) 
            {
                using (LogContext.PushProperty("LogProperty", new 
                       {
                           userId = id
                       }))
                {
                    _logger.LogInformation("회원 삭제: 값이 정수가 아님");
                }
                    
                return (false, 10081);
            }
                
            var data = await _db.Users
                .Where(p => p.Deleted == false
                            && p.UserId == id
                )
                .FirstOrDefaultAsync();

            if (data == null)
            {
                using (LogContext.PushProperty("LogProperty", new 
                       {
                           userId = id
                       }))
                {
                    _logger.LogInformation("회원 삭제: 삭제할 대상이 없음");
                }
                    
                return (false, 10082);
            }

            data.Deleted = true;

            await _db.SaveChangesAsync();
            
            return (true, 0);
        }
        catch (Exception ex)
        {
            using (LogContext.PushProperty("LogProperty", new 
                   {
                       userId = id
                   }))
            {
                _logger.LogError(ex, "DB 회원 삭제 중 오류 발생");
            }
                
            return (false, 1008);
        }
    }
}