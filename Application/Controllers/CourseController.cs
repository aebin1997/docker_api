using Application.Models.Course;
using Application.Models.User;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Serilog.Context;

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
    
    // TODO: [20221221-코드리뷰-33번-확인] Route와 Http Method Attribute 하나로 통합해주세요. - done
    // TODO: [20221221-코드리뷰-34번-확인] Http Request Class 명칭을 수정해주세요. - done
    [HttpGet("longest")]
    public async Task<ActionResult> GetLongestListByCourse([FromQuery] GetLongestListByCourseHttpRequest model)
    {
        model.Page = model.Page == 0 ? 1 : model.Page;
        
        // TODO: [20221221-코드리뷰-37번-확인] UserController에서 사용하신것처럼 Service Request class 변환 방식으로 수정해주세요. - done
        var result = await _course.GetLongestListByCourse(model.ToGetLongestListByCourse());
        
        if (result.isSuccess == false)
        { 
            var badRequestErrorCode = new int[] { 20001, 20002 };
            var serverErrorCode = new int[] { 2000 }; 
            
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
                    _logger.LogError("코스 별 롱기스트 조회에서 정의되지 않은 에러코드가 반환됨"); 
                }
                
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    
        var responseModel = new GetLongestListByCourseHttpResponse(result.response.List);
        
        return StatusCode(StatusCodes.Status200OK, responseModel);          
    }
    
    // TODO: [20221221-코드리뷰-35번-확인] Route와 Http Method Attribute 하나로 통합해주세요. - done
    // TODO: [20221221-코드리뷰-36번-확인] Http Request Class 명칭을 수정해주세요. - done
    [HttpGet("score")]
    public async Task<ActionResult> GetScoreListByCourse([FromQuery] GetScoreListByCourseHttpRequest model)
    {
        model.Page = model.Page == 0 ? 1 : model.Page;
        
        // TODO: [20221221-코드리뷰-38번-확인] UserController에서 사용하신것처럼 Service Request class 변환 방식으로 수정해주세요. - done
        var result = await _course.GetScoreListByCourse(model.ToGetScoreListByCourse());
        
        if (result.isSuccess == false)
        { 
            var badRequestErrorCode = new int[] { 20011, 20012 };
            var serverErrorCode = new int[] { 2001 }; 
            
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
                    _logger.LogError("코스 별 스코어 조회에서 정의되지 않은 에러코드가 반환됨"); 
                }
                
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    
        var responseModel = new GetScoreListByCourseHttpResponse(result.response.List);
        
        return StatusCode(StatusCodes.Status200OK, responseModel);  
    }
}