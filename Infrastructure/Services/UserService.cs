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
    
    public DateTime UnixTimeToDateTime(ulong unixTime)
    {
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            
        dateTime = dateTime.AddMilliseconds(unixTime).ToUniversalTime();
    
        return dateTime;
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

            // TODO: [20221219-코드리뷰-17번-확인] select하는 쿼리는 비추적 쿼리로 작성하셔야합니다. - done
            
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
            
            // TODO: [20221220-코드리뷰-28번-확인] Database에 조회 요청하는 부분이 잘못되었습니다. 원인을 찾은 후 수정해주세요. - done
            // TODO: [20221221-코드리뷰-30번] Database에 데이터 요청을 보낼 때 페이징 조건도 포함하여 전송되도록 로직을 수정해주세요.
            var dataList = await query
                .Select(p => new UserBestRecordListItem
                {
                    UserId = p.UserId,
                    Name = p.Name,
                    Score = p.Score,
                    Longest = p.Longest
                }).ToListAsync();
                
            var dataPageList = dataList.OrderByDescending(p => p.UserId)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

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

            // TODO: [20221221-코드리뷰-31번] Database에 데이터 요청을 보낼 때 페이징 조건도 포함하여 전송되도록 로직을 수정해주세요.
            var dataList = await query
                .GroupBy(p => p.UserId)
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
            
            var dataPageList = dataList.OrderByDescending(p => p.UserId)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();
            
            var response = new GetUserCourseHistoryListResponse
            {
                List = dataPageList
            };

            // response.List = (from data in dataPageList
            //     select new UserCourseHistoryListItem
            //     {
            //         UserId = data.UserId,
            //         List = (from course in data.CourseGroup
            //                 select new UserCourseHistoryItem
            //                 {
            //                     CourseId = course.CourseId,
            //                     Score = course.Score,
            //                     Longest = course.Longest
            //                 }).ToList()
            //     }).ToList();
            
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
            
            // TODO: [20221221-코드리뷰-32번] Database에 데이터 요청을 보낼 때 페이징 조건도 포함하여 전송되도록 로직을 수정해주세요.
            var dataList = await query
                .GroupBy(p => p.UserId)
                .Select(p => new UserClubInfoListItem
                {
                    UserId = p.Key,
                    List = p.Select(s => new UserClubInfoItem
                    {
                        Club = s.Club,
                        Distance = s.Distance
                    }).ToList()
                }).ToListAsync();

            var dataPageList = dataList.OrderByDescending(p => p.UserId)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();
            
            // TODO: [20221220-코드리뷰-29번-확인] 변수를 잘못 입력하셨습니다. 원인을 찾은 후 수정해주세요. - done
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
                    
            var userList = await query
                .Select(p => new
                {
                    p.UserId, p.Username, p.Password, p.Name, p.Created, p.Updated, p.Deleted
                })
                .ToListAsync();

            var pageList = userList.OrderByDescending(p => p.UserId)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();
            
            var response = new UserListResponse();
            response.TotalCount = userList.Count;
            response.List = (from user in pageList
                select new UserListItem
                {
                    UserId = user.UserId,
                    Username = user.Username,
                    Created = UnixTimeToDateTime(user.Created),
                    Updated = UnixTimeToDateTime(user.Updated),
                    Deleted = user.Deleted
                }).ToList();

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
            details.Created = UnixTimeToDateTime(data.Created);
            details.Updated = UnixTimeToDateTime(data.Updated);
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