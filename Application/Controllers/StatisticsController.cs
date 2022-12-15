using System.ComponentModel.DataAnnotations;
using Application.Models.Statistics;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers;

[Route("[controller]")]
[ApiController]
public class StatisticsController : ControllerBase
{
    // Log
    private readonly ILogger<UserController> _logger;
    
    // Service 
    private readonly IStatisticsService _statistics;
    
    public StatisticsController(ILogger<UserController> logger, IStatisticsService statistics)
    {
        _logger = logger;
    
        _statistics = statistics;
    }
    
    [Route("course/user/score/range")]
    [HttpGet]
    public async Task<ActionResult> GetUserScoreRangeByCourse([FromRoute] int userId)
    {
        var result = await _statistics.GetUserScoreRangeByCourse(userId);
        
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
                _logger.LogError("회원 상세 조회에서 정의되지 않은 에러코드가 반환됨"); 
                
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    
        var responseModel = new GetUserScoreRangeByCourseHttpResponse(result.response);
        
        return StatusCode(StatusCodes.Status200OK, responseModel);          
    }
    
    [Route("course/user/longest/range")]
    [HttpGet]
    public async Task<ActionResult> GetLongestListByCourse([FromRoute] int userId)
    {
        var result = await _statistics.GetUserLongestRangeByCourse(userId);
        
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
                _logger.LogError("회원 상세 조회에서 정의되지 않은 에러코드가 반환됨"); 
                
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    
        var responseModel = new GetUserLongestRangeByCourseHttpResponse(result.response);
        
        return StatusCode(StatusCodes.Status200OK, responseModel);          
    }
    
    [Route("course/rounding/count")]
    [HttpGet]
    public async Task<ActionResult> GetCourseRoundingCount([FromRoute] int userId)
    {
        var result = await _statistics.GetCourseRoundingCount(userId);
        
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
                _logger.LogError("회원 상세 조회에서 정의되지 않은 에러코드가 반환됨"); 
                
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    
        var responseModel = new GetCourseRoundingCountHttpResponse(result.response);
        
        return StatusCode(StatusCodes.Status200OK, responseModel);          
    }
    
    [Route("course/rounding/count/year")]
    [HttpGet]
    public async Task<ActionResult> GetCourseRoundingCountByYear([FromRoute] int userId)
    {
        var result = await _statistics.GetCourseRoundingCountByYear(userId);
        
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
                _logger.LogError("회원 상세 조회에서 정의되지 않은 에러코드가 반환됨"); 
                
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    
        var responseModel = new GetCourseRoundingCountByYearHttpResponse(result.response);
        return StatusCode(StatusCodes.Status200OK, responseModel);          
    }
    
    [Route("course/rounding/count/month")]
    [HttpGet]
    public async Task<ActionResult> GetCourseRoundingCountByMonth([FromRoute] int userId)
    {
        var result = await _statistics.GetCourseRoundingCountByMonth(userId);
        
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
                _logger.LogError("회원 상세 조회에서 정의되지 않은 에러코드가 반환됨"); 
                
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    
        var responseModel = new GetCourseRoundingCountByMonthHttpResponse(result.response);
        
        return StatusCode(StatusCodes.Status200OK, responseModel);          
    }
    
    [Route("course/bestScore")]
    [HttpGet]
    public async Task<ActionResult> GetBestScoreByCourse()
    {
        var result = await _statistics.GetBestScoreByCourse();
        
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
                _logger.LogError("회원 상세 조회에서 정의되지 않은 에러코드가 반환됨"); 
                
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    
        var responseModel = new GetBestScoreByCourseHttpResponse(result.response.List);
        
        return StatusCode(StatusCodes.Status200OK, responseModel);          
    }
    
    [Route("course/longest")]
    [HttpGet]
    public async Task<ActionResult> GetLongestByCourse()
    {
        var result = await _statistics.GetLongestByCourse();
        
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
                _logger.LogError("회원 상세 조회에서 정의되지 않은 에러코드가 반환됨"); 
                
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    
        var responseModel = new GetLongestByCourseHttpResponse(result.response.List);
        
        return StatusCode(StatusCodes.Status200OK, responseModel);          
    }
}