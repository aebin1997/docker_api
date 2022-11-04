using Infrastructure.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Services;
using Application.Models.User.Request;
using Newtonsoft.Json.Linq;
using Serilog.Context;
using Application.Models.User.Response;

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

    [HttpGet]
    // TODO: Http Request 모델 추가, 필터 기능 추가(paging 기능 추가(PageSize 조절 가능), LifeBestScore 검색 추가 (이상, 이하)
    public async Task<ActionResult> GetUsers([FromQuery] UsersListParameterModel model)
    {
        model.Page = model.Page == 0 ? 1 : model.Page;
        
        // TODO: Service에 넘길 때 Model을 파싱하여 전달해주세요.
        var result = await _user.GetUsers(
            model.Page,
            model.PageSize
        );

        if (result.isSuccess == false)
        {
            // TODO: 필터 데이터에 대한 유효성 검사로 인해 반환되는 에러는 400 Error로 반환
            // TODO: try-catch로 반환되는 에러는 500 Error로 반환
            using (LogContext.PushProperty("JsonData", new
                   {
                       model = JObject.FromObject(model)
                   }))
            {
                _logger.LogError("회원 목록 조회 실패 에러코드가 반환됨"); 
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        // TODO: HttpRequest model 새로 생성하여 반환 처리하도록 수정해주세요.
        var responseModel = new UserListHttpResponse(result.response);
        
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

        // UserListResponse responseModel = new UserListResponse(result.totalCount, result.list);
        UserListResponse responseModel = new UserListResponse();
    
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

        // UserListResponse responseModel = new UserListResponse(result.totalCount, result.list);
        UserListResponse responseModel = new UserListResponse();

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
        var result = await _user.AddUser(model.ToAddUserRequest());

        if (result.isSuccess == false)
        {
            var badRequestErrorCode = new int[] { 1000, 1001 };
            var serverErrorCode = new int[] { 1002 };
            
            // TODO: 입력할 데이터에 대한 유효성 검사로 인해 반환되는 에러는 Http Status 400 Error로 반환
            // TODO: try-catch로 반환되는 에러는 500 Error로 반환
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
                using (LogContext.PushProperty("JsonData", new
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
