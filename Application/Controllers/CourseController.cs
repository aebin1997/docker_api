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
    
    // TODO: [20221221-코드리뷰-33번] Route와 Http Method Attribute 하나로 통합해주세요.
    // TODO: [20221221-코드리뷰-34번] Http Request Class 명칭을 수정해주세요.
    [Route("longest")]
    [HttpGet]
    public async Task<ActionResult> GetLongestListByCourse([FromQuery] PagingHttpRequest model)
    {
        model.Page = model.Page == 0 ? 1 : model.Page;
        
        // TODO: [20221221-코드리뷰-37번] UserController에서 사용하신것처럼 Service Request class 변환 방식으로 수정해주세요.
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
    
    // TODO: [20221221-코드리뷰-35번] Route와 Http Method Attribute 하나로 통합해주세요.
    // TODO: [20221221-코드리뷰-36번] Http Request Class 명칭을 수정해주세요.
    [Route("score")]
    [HttpGet]
    public async Task<ActionResult> GetScoreListByCourse([FromQuery] PagingHttpRequest model)
    {
        model.Page = model.Page == 0 ? 1 : model.Page;
        
        // TODO: [20221221-코드리뷰-38번] UserController에서 사용하신것처럼 Service Request class 변환 방식으로 수정해주세요.
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