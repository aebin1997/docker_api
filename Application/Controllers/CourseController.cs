using Application.Models.Course;
using Application.Models.User;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers;

[Route("[controller]")]
[ApiController]
public class CourseController : ControllerBase
{
    // Log
    private readonly ILogger<CourseController> _logger;

    // Service 
    private readonly ICourseService _course;

    public CourseController(ILogger<CourseController> logger, ICourseService course)
    {
        _logger = logger;
    
        _course = course;
    }
    
    [Route("longest")]
    [HttpGet]
    public async Task<ActionResult> GetLongestListByCourse([FromQuery] PagingHttpRequest model)
    {
        model.Page = model.Page == 0 ? 1 : model.Page;
        
        var result = await _course.GetLongestListByCourse(model.Page, model.PageSize, model.CourseId);
        
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
    
        var responseModel = new GetLongestListByCourseHttpResponse(result.response.List);
        
        return StatusCode(StatusCodes.Status200OK, responseModel);          
    }
    
    [Route("score")]
    [HttpGet]
    public async Task<ActionResult> GetScoreListByCourse([FromQuery] PagingHttpRequest model)
    {
        model.Page = model.Page == 0 ? 1 : model.Page;
        
        var result = await _course.GetScoreListByCourse(model.Page, model.PageSize, model.CourseId);
        
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
    
        var responseModel = new GetScoreListByCourseHttpResponse(result.response.List);
        
        return StatusCode(StatusCodes.Status200OK, responseModel);  
    }
}