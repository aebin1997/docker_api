using Microsoft.AspNetCore.Mvc;
using Infrastructure.Services;
using Application.Models.User;
using Application.Models.User.Request;
using Infrastructure.Models.User;

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

    [Route("best")]
    [HttpGet]
    public async Task<ActionResult> GetUserBestRecordList()
    {
        var result = await _user.GetUserBestRecordList();

        if (result.isSuccess == false)
        {
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
        
        var responseModel = new GetUserBestRecordListHttpResponse(result.response.List);
            
        return StatusCode(StatusCodes.Status200OK, responseModel);
    }
    
    [Route("course")]
    [HttpGet]
    public async Task<ActionResult> GetUserCourseHistoryList([FromQuery] GetUserCourseHistoryListHttpRequest model)
    {
        model.Page = model.Page == 0 ? 1 : model.Page;
        
        var result = await _user.GetUserCourseHistoryList(model.Page, model.PageSize);
        
        if (result.isSuccess == false)
        { 
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
    
        var responseModel = new GetUserCourseHistoryListHttpResponse(result.response.List);
        
        return StatusCode(StatusCodes.Status200OK, responseModel);          
    }
    
    [Route("club")]
    [HttpGet]
    public async Task<ActionResult> GetUserClubInfoList()
    {
        var result = await _user.GetUserClubInfoList();
        
        if (result.isSuccess == false)
        { 
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
    
        var responseModel = new GetUserClubInfoListHttpResponse(result.response.List);
        
        return StatusCode(StatusCodes.Status200OK, responseModel);  
    }
}
