using Domain.Error;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Services;
using Application.Models.User.Request;
using Newtonsoft.Json.Linq;
using Serilog.Context;
using Application.Models.User.Response;
using Newtonsoft.Json;

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
    public async Task<ActionResult> GetUsers([FromQuery] GetUsersHttpRequest model)
    {
        using (LogContext.PushProperty("LogProperty", new 
               {
                   model = JsonConvert.SerializeObject(model)
               }))
        {
            _logger.LogInformation("로그 테스트");
        }                    
        model.Page = model.Page == 0 ? 1 : model.Page;
        
        var result = await _user.GetUsers(model.ToGetUsersRequest());

        if (result.isSuccess == false)
        {
            // TODO: [20221106-권용진] 1번 (수정 완료)
            // TODO: [20221106-권용진] 2022년 10월 27일에 적어둔 사항이 좀 애매한 부분이 있어 정정합니다.
            // TODO: [20221106-권용진] 요청으로 들어온 데이터가 잘못된 형식일 경우에는 HTTP Status Code가 400으로 반환
            // TODO: [20221106-권용진] 비즈니스 로직 오류 또는 의도치 않은 에러가 발생되면 HTTP Status Code가 500으로 반환
            // TODO: [20221106-권용진] 회원 목록 조회 서비스에서 반환되는 에러코드가 아닌 다른 에러 코드가 넘어오면 로그를 남기고 HTTP Status Code가 500으로 반환
            var badRequestErrorCode = new int[] { 1000 };
            var serverErrorCode = new int[] { 1001 }; 
            
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
    
    [HttpGet("{idx}")]
    public async Task<ActionResult> GetUserDetails([FromRoute] int idx)
    {
        var result = await _user.GetUserDetails(idx);
        
        if (result.isSuccess == false)
        { 
            // TODO: [20221106-권용진] 1번과 동일하게 처리해주시기 바랍니다. (완료)
            var badRequestErrorCode = new int[] { 1002 };
            var serverErrorCode = new int[] { 1003, 1004 }; 
            
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
                // using (LogContext.PushProperty("LogProperty", new
                //        {
                //            idx = JObject.FromObject(idx)
                //        }))
                // {
                //     _logger.LogError("회원 상세 조회에서 정의되지 않은 에러코드가 반환됨"); 
                // }

                _logger.LogError("회원 상세 조회에서 정의되지 않은 에러코드가 반환됨"); 
                
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
            var badRequestErrorCode = new int[] { 1005, 1006 };
            // TODO: [박예빈] db 조회 실패를 500으로 두고 따로 표기해야하는지 (해결)
            // TODO: [20221106-권용진] 서비스 메서드에서 반환되는 에러코드는 고유한 코드입니다. DB 조회 실패를 특정해서 처리하는게 아닙니다.
            // TODO: [20221106-권용진] 현재 작성된 코드에선 에러코드가 500으로 반환되어 배열에 500만 들어가 있지만 서버에 대한 오류로 인해 넘어오는 에러코드가 여러개일 경우 해당 배열에는 에러코드가 여러개가 들어가 있습니다.
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
                // using (LogContext.PushProperty("LogProperty", new
                //        {
                //            model = JObject.FromObject(model)
                //        }))
                // {
                //     _logger.LogError("회원 추가에 대한 결과로 정의되지 않은 에러코드가 반환됨"); 
                // }
                
                _logger.LogError("회원 추가에 대한 결과로 정의되지 않은 에러코드가 반환됨"); 
                
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        return StatusCode(StatusCodes.Status201Created);
    }

    [HttpPut("{idx}")]
    // TODO: [20221106-권용진] 11번 (답변 완료)
    // TODO: [20221106-권용진] 업데이트의 경우 데이터를 Body가 아닌 Form으로 받으신 이유가 무엇인가요 ? - 음.. 그냥 실수인거같은데 [FromForm] 은 클라이언트에서 form 썼을 때만 쓰는거 맞죠?  
    public async Task<ActionResult> PutUser([FromRoute] int idx, [FromBody] UpdateUserHttpRequest model)
    {
        var result = await _user.UpdateUser(model.ToUpdateUserHttpRequest(idx));

        if (result.isSuccess == false)
        {
            var badRequestErrorCode = new int[] { 1008, 1010, 1011 };
            var serverErrorCode = new int[] { 1009, 1012 }; 
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
                // using (LogContext.PushProperty("LogProperty", new
                //        {
                //            idx = JObject.FromObject(idx)
                //        }))
                // {
                //     _logger.LogError("회원 정보 수정중 정의되지 않은 에러코드가 반환됨"); 
                // }

                _logger.LogError("회원 정보 수정중 정의되지 않은 에러코드가 반환됨"); 
                
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // TODO: [20221106-권용진] 15번 (답변 완료)
        // TODO: [20221106-권용진] 회원 수정 완료 후 응답에 대한 회원 데이터를 별도로 조회하여 반환한 이유가 무엇인가요 ? - 그냥 제가 수정 잘됐는지 확인하려고 했습니다.
        var result2 = await _user.GetUserDetails(idx);

            if (result2.isSuccess == false)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            var responseModel = new UserDetailsHttpResponse(result2.details);
            return StatusCode(StatusCodes.Status200OK, responseModel);
        }

    [HttpDelete("{idx}")]
    public async Task<ActionResult> DeleteUser([FromRoute] int idx)
    {
        var result = await _user.DeleteUser(idx);
        
        if (result.isSuccess == false)
        {
            var badRequestErrorCode = new int[] { 1013 };
            var serverErrorCode = new int[] { 1014, 1015 }; 
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
                // using (LogContext.PushProperty("LogProperty", new
                //        {
                //            idx = JObject.FromObject(idx)
                //        }))
                // {
                //     _logger.LogError("회원 삭제중 정의되지 않은 에러코드가 반환됨"); 
                // }

                _logger.LogError("회원 삭제중 정의되지 않은 에러코드가 반환됨"); 
                
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        
        return StatusCode(StatusCodes.Status200OK);
    }
}
