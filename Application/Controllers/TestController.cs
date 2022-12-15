using Microsoft.AspNetCore.Mvc;
using Infrastructure.Services;

namespace Application.Controllers;

[Route("[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    // Log
    private readonly ILogger<TestController> _logger;

    // Service 
    private readonly ITestService _test;

    public TestController(ILogger<TestController> logger, ITestService test)
    {
        _logger = logger;

        _test = test;
    }
    
    [Route("user")]
    [HttpPost]
    public async Task<ActionResult> AddUsers()
    {
        var result = await _test.AddUsers();

        if (result.isSuccess == false)
        {
            var badRequestErrorCode = new int[] { };
            var serverErrorCode = new int[] { }; 
            
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
                // _logger.LogError("회원 추가에 대한 결과로 정의되지 않은 에러코드가 반환됨"); 
                
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        return StatusCode(StatusCodes.Status201Created);
    }
    
    [Route("course")]
    [HttpPost]
    public async Task<ActionResult> AddCourses()
    {
        var result = await _test.AddCourses();

        if (result.isSuccess == false)
        {
            var badRequestErrorCode = new int[] { };
            var serverErrorCode = new int[] { }; 
            
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
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        return StatusCode(StatusCodes.Status201Created);
    }
    
    [Route("byCourse")]
    [HttpPost]
    public async Task<ActionResult> AddUserByCourse()
    {
        var result = await _test.AddUserByCourse();

        if (result.isSuccess == false)
        {
            var badRequestErrorCode = new int[] { };
            var serverErrorCode = new int[] { }; 
            
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
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        return StatusCode(StatusCodes.Status201Created);
    }
    
    [Route("byClub")]
    [HttpPost]
    public async Task<ActionResult> AddUserByClub()
    {
        var result = await _test.AddUserByClub();

        if (result.isSuccess == false)
        {
            var badRequestErrorCode = new int[] { };
            var serverErrorCode = new int[] { }; 
            
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
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        return StatusCode(StatusCodes.Status201Created);
    }
    
    [Route("bestRecord")]
    [HttpPost]
    public async Task<ActionResult> AddBestRecord()
    {
        var result = await _test.AddBestRecord();

        if (result.isSuccess == false)
        {
            var badRequestErrorCode = new int[] { };
            var serverErrorCode = new int[] { }; 
            
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
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        return StatusCode(StatusCodes.Status201Created);
    }
}
