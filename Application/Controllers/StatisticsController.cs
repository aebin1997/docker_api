using System.ComponentModel.DataAnnotations;
using Application.Models.Course;
using Application.Models.Statistics;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Serilog.Context;

namespace Application.Controllers;

[Route("[controller]")]
[ApiController]
public class StatisticsController : ControllerBase
{
    // TODO: [20221222-코드리뷰-43번] ActionMethod Route, HTTP Method 통합해주세요 - done

    // Log
    private readonly ILogger<UserController> _logger;
    
    // Service 
    private readonly IStatisticsService _statistics;
    
    public StatisticsController(ILogger<UserController> logger, IStatisticsService statistics)
    {
        _logger = logger;
    
        _statistics = statistics;
    }

    [HttpGet("users/{userId}/course-score-range")]
    public async Task<ActionResult> GetUserScoreRangeByCourse([FromRoute] int userId, [FromQuery] GetUserScoreRangeByCourseHttpRequest model)
    {
        var result = await _statistics.GetUserScoreRangeByCourse(model.ToGetUserScoreRangeByCourse(userId));
        
        if (result.isSuccess == false)
        { 
            var badRequestErrorCode = new int[] { 30001, 30002 };
            var serverErrorCode = new int[] { 3000 }; 
            
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
                    _logger.LogError("코스별로 최고 스코어 데이터 조회 중 정의되지 않은 에러코드가 반환됨"); 
                }                
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    
        var responseModel = new GetUserScoreRangeByCourseHttpResponse(result.response);
        
        return StatusCode(StatusCodes.Status200OK, responseModel);          
    }
    
    [HttpGet("users/{userId}/course-longest-range")]
    public async Task<ActionResult> GetUserLongestRangeByCourse([FromRoute] int userId, [FromQuery] GetUserLongestRangeByCourseHttpRequest model)
    {
        var result = await _statistics.GetUserLongestRangeByCourse(model.ToGetUserLongestRangeByCourse(userId));
        
        if (result.isSuccess == false)
        { 
            var badRequestErrorCode = new int[] { 30011, 30012 };
            var serverErrorCode = new int[] { 3001 }; 
            
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
                           model = JObject.FromObject(model),
                       }))
                {
                    _logger.LogError("코스별로 최고 롱기스트 거리 데이터 조회 중 정의되지 않은 에러코드가 반환됨"); 
                }   
                
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    
        var responseModel = new GetUserLongestRangeByCourseHttpResponse(result.response);
        
        return StatusCode(StatusCodes.Status200OK, responseModel);          
    }
    
    [HttpGet("courses/rounding-count")]
    public async Task<ActionResult> GetCourseRoundingCount([FromQuery] GetCourseRoundingCountHttpRequest model)
    {
        var result = await _statistics.GetCourseRoundingCount(model.ToGetCourseRoundingCount());
        
        if (result.isSuccess == false)
        { 
            var badRequestErrorCode = new int[] { 30021, 30022};
            var serverErrorCode = new int[] { 3002 }; 
            
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
                           model = JObject.FromObject(model),
                       }))
                {
                    _logger.LogError("특정 코스의 라운딩 카운트 조회 중 정의되지 않은 에러코드가 반환됨"); 
                }   
                
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    
        var responseModel = new GetCourseRoundingCountHttpResponse(result.response);
        
        return StatusCode(StatusCodes.Status200OK, responseModel);          
    }
    
    [HttpGet("courses/rounding-count/year")]
    public async Task<ActionResult> GetCourseRoundingCountByYear([FromQuery] GetCourseRoundingCountByYearHttpRequest model)
    {
        var result = await _statistics.GetCourseRoundingCountByYear(model.ToGetCourseRoundingCountByYear());
        
        if (result.isSuccess == false)
        { 
            var badRequestErrorCode = new int[] { 30031, 30032 };
            var serverErrorCode = new int[] { 3003 }; 
            
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
                           model = JObject.FromObject(model),
                       }))
                {
                    _logger.LogError("특정 코스의 연도별 라운딩 카운트 조회 중 정의되지 않은 에러코드가 반환됨"); 
                }   
                
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    
        var responseModel = new GetCourseRoundingCountByYearHttpResponse(result.response);
        return StatusCode(StatusCodes.Status200OK, responseModel);          
    }
    
    [HttpGet("courses/rounding-count/month")]
    public async Task<ActionResult> GetCourseRoundingCountByMonth([FromQuery] GetCourseRoundingCountByMonthHttpRequest model)
    {
        var result = await _statistics.GetCourseRoundingCountByMonth(model.ToGetCourseRoundingCountByMonth());
        
        if (result.isSuccess == false)
        { 
            var badRequestErrorCode = new int[] { 30041, 30042 };
            var serverErrorCode = new int[] { 3004 }; 
            
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
                           model = JObject.FromObject(model),
                       }))
                {
                    _logger.LogError("특정 코스의 월별 라운딩 카운트 조회 중 정의되지 않은 에러코드가 반환됨"); 
                }   
                
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    
        var responseModel = new GetCourseRoundingCountByMonthHttpResponse(result.response);

        return StatusCode(StatusCodes.Status200OK, responseModel);          
    }
    
    [HttpGet("courses/best-score")]
    public async Task<ActionResult> GetBestScoreByCourse([FromQuery] GetBestScoreByCourseHttpRequest model)
    {
        var result = await _statistics.GetBestScoreByCourse(model.ToGetBestScoreByCourse());
        
        if (result.isSuccess == false)
        { 
            var badRequestErrorCode = new int[] { 30051, 30052 };
            var serverErrorCode = new int[] { 3005 }; 
            
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
                           model = JObject.FromObject(model),
                       }))
                {
                    _logger.LogError("코스별로 최고 스코어 데이터 조회 중 정의되지 않은 에러코드가 반환됨"); 
                }   
                
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    
        var responseModel = new GetBestScoreByCourseHttpResponse(result.response);
        
        return StatusCode(StatusCodes.Status200OK, responseModel);          
    }
    
    [HttpGet("courses/longest")]
    public async Task<ActionResult> GetLongestByCourse([FromQuery] GetLongestByCourseHttpRequest model)
    {
        var result = await _statistics.GetLongestByCourse(model.ToGetLongestByCourse());
        
        if (result.isSuccess == false)
        { 
            var badRequestErrorCode = new int[] { 30061, 30062 };
            var serverErrorCode = new int[] { 3006 }; 
            
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
                           model = JObject.FromObject(model),
                       }))
                {
                    _logger.LogError("코스별로 최고 롱기스트 거리 데이터 조회 중 정의되지 않은 에러코드가 반환됨"); 
                }   
                
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    
        var responseModel = new GetLongestByCourseHttpResponse(result.response);
        
        return StatusCode(StatusCodes.Status200OK, responseModel);          
    }
}