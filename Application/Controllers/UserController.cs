using Microsoft.AspNetCore.Mvc;
using Infrastructure.Services;
using Application.Models.User;
using Application.Models.User.Request;
using Infrastructure.Models.User;
using Newtonsoft.Json.Linq;
using Serilog.Context;

namespace Application.Controllers;

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

    [HttpGet("best")]
    public async Task<ActionResult> GetUserBestRecordList([FromQuery] GetUserBestRecordListHttpRequest model)
    {
        model.Page = model.Page == 0 ? 1 : model.Page;

        var result = await _user.GetUserBestRecordList(model.GetUserBestRecordList());

        if (result.isSuccess == false)
        {
            var badRequestErrorCode = new int[] { 10001, 10002 };
            var serverErrorCode = new int[] { 1000 };
            
            if (badRequestErrorCode.Contains(result.errorCode))
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            else if (serverErrorCode.Contains(result.errorCode))
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            else
            {
                _logger.LogError("회원 별 최고기록 조회 중 정의되지 않은 에러코드가 반환됨"); 

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        
        var responseModel = new GetUserBestRecordListHttpResponse(result.response.List);
            
        return StatusCode(StatusCodes.Status200OK, responseModel);
    }
    
    // TODO: [20221220-코드리뷰-24번] Route와 Http Method를 선언한 Attribute를 하나로 처리해주세요. 
    // TODO: [20221220-코드리뷰-25번] Http Method의 경우 데이터를 받아올 때 Body가 아닌 Query String으로 받아와야합니다.
    [Route("course")]
    [HttpGet]
    public async Task<ActionResult> GetUserCourseHistoryList([FromBody] GetUserCourseHistoryListHttpRequest model)
    {
        model.Page = model.Page == 0 ? 1 : model.Page;
        
        var result = await _user.GetUserCourseHistoryList(model.GetUserCourseHistoryList());
        
        if (result.isSuccess == false)
        { 
            var badRequestErrorCode = new int[] { 10021 };
            var serverErrorCode = new int[] { 1002 }; 
            
            if (badRequestErrorCode.Contains(result.errorCode))
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            else if (serverErrorCode.Contains(result.errorCode))
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            else
            {
                using (LogContext.PushProperty("LogProperty", new
                       {
                           model = JObject.FromObject(model)
                       }))
                {
                    _logger.LogError("회원 별 코스기록 조회 중 정의되지 않은 에러코드가 반환됨"); 
                }
                
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    
        var responseModel = new GetUserCourseHistoryListHttpResponse(result.response.List);
        
        return StatusCode(StatusCodes.Status200OK, responseModel);          
    }
    
    // TODO: [20221220-코드리뷰-26번] Route와 Http Method를 선언한 Attribute를 하나로 처리해주세요.
    // TODO: [20221220-코드리뷰-27번] Http Method의 경우 데이터를 받아올 때 Body가 아닌 Query String으로 받아와야합니다.
    [Route("club")]
    [HttpGet]
    public async Task<ActionResult> GetUserClubInfoList([FromBody] GetUserClubInfoListHttpRequest model)
    {
        model.Page = model.Page == 0 ? 1 : model.Page;
        
        var result = await _user.GetUserClubInfoList(model.GetUserClubInfoList());
        
        if (result.isSuccess == false)
        { 
            var badRequestErrorCode = new int[] { 10031, 10032 };
            var serverErrorCode = new int[] { 1003 }; 
            
            if (badRequestErrorCode.Contains(result.errorCode))
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            else if (serverErrorCode.Contains(result.errorCode))
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            else
            {
                _logger.LogError("회원 별 클럽 거리 정보 조회 중 정의되지 않은 에러코드가 반환됨"); 
                
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    
        var responseModel = new GetUserClubInfoListHttpResponse(result.response.List);
        
        return StatusCode(StatusCodes.Status200OK, responseModel);  
    }
    
    [HttpGet]
    public async Task<ActionResult> GetUsers([FromQuery] GetUsersHttpRequest model)
    {
        model.Page = model.Page == 0 ? 1 : model.Page;
        
        var result = await _user.GetUsers(model.ToGetUsersRequest());

        if (result.isSuccess == false)
        {
            var badRequestErrorCode = new int[] { 10041 };
            var serverErrorCode = new int[] { 1004 }; 
            
            if (badRequestErrorCode.Contains(result.errorCode))
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            else if (serverErrorCode.Contains(result.errorCode))
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            else
            {
                _logger.LogError("회원 목록 조회에서 정의되지 않은 에러코드가 반환됨"); 

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        
        var responseModel = new UserListHttpResponse(result.response);
        
        return StatusCode(StatusCodes.Status200OK, responseModel);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult> GetUserDetails([FromRoute] int id)
    {
        var result = await _user.GetUserDetails(id);
        
        if (result.isSuccess == false)
        { 
            var badRequestErrorCode = new int[] { 10051, 10052 };
            var serverErrorCode = new int[] { 1005 }; 
            
            if (badRequestErrorCode.Contains(result.errorCode))
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            else if (serverErrorCode.Contains(result.errorCode))
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            else
            {
                using (LogContext.PushProperty("LogProperty", new
                       {
                           id = id
                       }))
                {
                    _logger.LogError("회원 상세 조회에서 정의되지 않은 에러코드가 반환됨");  
                }
                
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        var responseModel = new UserDetailsHttpResponse(result.details);
        return StatusCode(StatusCodes.Status200OK, responseModel);          
    }
    
    [HttpPost]
    public async Task<ActionResult> PostUser([FromBody] AddUserHttpRequest model)
    {
        var result = await _user.AddUser(model.ToAddUserRequest());

        if (result.isSuccess == false)
        {
            var badRequestErrorCode = new int[] { 10061, 10062, 10063 };
            var serverErrorCode = new int[] { 1006 }; 
            
            if (badRequestErrorCode.Contains(result.errorCode))
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            else if (serverErrorCode.Contains(result.errorCode))
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            else
            {
                using (LogContext.PushProperty("LogProperty", new
                       {
                           model = JObject.FromObject(model)
                       }))
                {
                    _logger.LogError("회원 추가에 대한 결과로 정의되지 않은 에러코드가 반환됨"); 
                }
                
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        return StatusCode(StatusCodes.Status201Created);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> PutUser([FromRoute] int id, [FromBody] UpdateUserHttpRequest model)
    {
        var result = await _user.UpdateUser(model.ToUpdateUserHttpRequest(id));

        if (result.isSuccess == false)
        {
            var badRequestErrorCode = new int[] { 10071, 10072, 10073, 10074 };
            var serverErrorCode = new int[] { 1007 }; 
            if (badRequestErrorCode.Contains(result.errorCode))
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            else if (serverErrorCode.Contains(result.errorCode))
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            else
            {
                using (LogContext.PushProperty("LogProperty", new
                       {
                           model = JObject.FromObject(model)
                       }))
                {
                    _logger.LogError("회원 정보 수정중 정의되지 않은 에러코드가 반환됨"); 
                }

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        var result2 = await _user.GetUserDetails(id);

        if (result2.isSuccess == false)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        var responseModel = new UserDetailsHttpResponse(result2.details);
        return StatusCode(StatusCodes.Status200OK, responseModel);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUser([FromRoute] int id)
    {
        var result = await _user.DeleteUser(id);
        
        if (result.isSuccess == false)
        {
            var badRequestErrorCode = new int[] { 10081, 10082 };
            var serverErrorCode = new int[] { 1008 }; 
            if (badRequestErrorCode.Contains(result.errorCode))
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            else if (serverErrorCode.Contains(result.errorCode))
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            else
            {
                using (LogContext.PushProperty("LogProperty", new
                       {
                           id = JObject.FromObject(id)
                       }))
                {
                    _logger.LogError("회원 삭제중 정의되지 않은 에러코드가 반환됨");
                }
                
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        
        return StatusCode(StatusCodes.Status200OK);
    }
}
