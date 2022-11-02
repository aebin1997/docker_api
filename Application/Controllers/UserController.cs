using Amazon.SimpleNotificationService.Model;
using Infrastructure.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Services;
using Application.Models.User.Request;
using Newtonsoft.Json.Linq;
using Serilog.Context;

namespace Application.Controllers
{
    [Route("[controller]")]
    [ApiController]
    
    public class UserController : ControllerBase
    {
        // Log
        private readonly ILogger<UserController> _logger;
    
        // Service 
        private readonly IUserService _user;

        public UserController(ILogger<UserController> logger, IUserService user)
        {
            _logger = logger;
        
            _user = user;
        }

        [HttpGet]
        public async Task<ActionResult> GetUsers([FromQuery] UsersListParameterModel model)
        {
            model.Page = model.Page == 0 ? 1 : model.Page;
            
            var result = await _user.GetUsers(
                model.Page,
                model.PageSize
            );

            if (result.isSuccess == false)
            {
                using (LogContext.PushProperty("JsonData", new
                       {
                           model = JObject.FromObject(model)
                       }))
                {
                    _logger.LogError("회원 목록 조회 실패 에러코드가 반환됨"); 
                }

                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            UserListResponse responseModel = new UserListResponse(result.totalCount, result.list);
            
            return StatusCode(StatusCodes.Status200OK, responseModel);
        }

        [HttpGet("above/{score}")]
        public async Task<ActionResult> GetUsersAbove([FromRoute] int score)
        {
            var result = await _user.GetUsersAbove(score);

            if (result.isSuccess == false)
            {
                using (LogContext.PushProperty("JsonData", new
                       {
                           score = JObject.FromObject(score)
                       }))
                {
                    _logger.LogError("특정 점수 이상 회원 조회 실패 에러코드가 반환됨"); 
                }

                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            UserListResponse responseModel = new UserListResponse(result.totalCount, result.list);
        
            return StatusCode(StatusCodes.Status200OK, responseModel);
        }
        
        [HttpGet("below/{score}")]
        public async Task<ActionResult> GetUsersBelow([FromRoute] int score)
        {
            var result = await _user.GetUsersBelow(score);

            if (result.isSuccess == false)
            {
                using (LogContext.PushProperty("JsonData", new
                       {
                           score = JObject.FromObject(score)
                       }))
                {
                    _logger.LogError("특정 점수 이하 회원 조회 실패 에러코드가 반환됨"); 
                }

                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            UserListResponse responseModel = new UserListResponse(result.totalCount, result.list);

            return StatusCode(StatusCodes.Status200OK, responseModel);
        }
        
        [HttpGet("{idx}")]
        public async Task<ActionResult> GetUserDetails([FromRoute] int idx)
        {
            var result = await _user.GetUserDetails(idx);
            
            if (result.isSuccess == false)
            {
                using (LogContext.PushProperty("JsonData", new
                       {
                           idx = JObject.FromObject(idx)
                       }))
                {
                    _logger.LogError("회원 상세 조회 실패 에러코드가 반환됨"); 
                }

                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return StatusCode(StatusCodes.Status200OK, result.details);          
        }
        
        [HttpPost]
        public async Task<ActionResult> PostUser([FromBody] AddUserHttpRequest model)
        {
            var result = await _user.AddUser(model.ToAddUserHttpRequest());

            if (result.IsSuccess == false)
            {
                if (result.ErrorCode == 400)
                {
                    using (LogContext.PushProperty("JsonData", new
                           {
                               model = JObject.FromObject(model)
                           }))
                    {
                        _logger.LogError("유효성 검사 실패 에러코드가 반환됨"); 
                    } 
                }
                else
                {
                    using (LogContext.PushProperty("JsonData", new
                           {
                               model = JObject.FromObject(model)
                           }))
                    {
                        _logger.LogError("회원 추가 실패 에러코드가 반환됨"); 
                    }
                }

                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPut("{idx}")]
        public async Task<ActionResult> PutUser([FromRoute] int idx, [FromForm] UpdateUserHttpRequest model)
        {
            var result = await _user.UpdateUser(model.ToUpdateUserHttpRequest(idx));

            if (result.isSuccess == false)
            {
                if (result.errorCode == 400)
                {
                    using (LogContext.PushProperty("JsonData", new
                           {
                               model = JObject.FromObject(model)
                           }))
                    {
                        _logger.LogError("존재하지 않는 아이디");
                    }
                }
                else
                {
                    using (LogContext.PushProperty("JsonData", new
                           {
                               model = JObject.FromObject(model)
                           }))
                    {
                        _logger.LogError("회원 수정 실패 에러코드가 반환됨");
                    }
                }

                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            var result2 = await _user.GetUserDetails(idx);

                if (result2.isSuccess == false)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }

                return StatusCode(StatusCodes.Status200OK, result2.details);
            }

        [HttpDelete("{idx}")]
        public async Task<ActionResult> DeleteUser([FromRoute] int idx)
        {
            var result = await _user.DeleteUser(idx);
            
            if (result.isSuccess == false)
            {
                if (result.errorCode == 400)
                {
                    using (LogContext.PushProperty("JsonData", new
                           {
                               idx = JObject.FromObject(idx)
                           }))
                    {
                        _logger.LogError("존재하지 않는 아이디");
                    }
                }
                else
                {
                    using (LogContext.PushProperty("JsonData", new
                           {
                               idx = JObject.FromObject(idx)
                           }))
                    {
                        _logger.LogError("회원 삭제 실패 에러코드가 반환됨");
                    }
                }

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            
            return StatusCode(StatusCodes.Status200OK);
        }
    }
}