using Application.Models.User;
using Domain.Entities;
using Infrastructure.Models.Request;
using Infrastructure.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Services;

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
    public async Task<ActionResult> GetUsers()
    {
        // TODO: Service에 넘길 때 Model을 파싱하여 전달해주세요.
        var result = await _user.GetUsers();

        if (result.isSuccess == false)
        {
            // TODO: 필터 데이터에 대한 유효성 검사로 인해 반환되는 에러는 400 Error로 반환
            // TODO: try-catch로 반환되는 에러는 500 Error로 반환
            Console.WriteLine("get user list fail");
            return StatusCode(StatusCodes.Status404NotFound, null);
        }

        // TODO: HttpRequest model 새로 생성하여 반환 처리하도록 수정해주세요.
        UserListResponse responseModel = new UserListResponse(result.totalCount, result.list);
        
        return StatusCode(StatusCodes.Status200OK, responseModel);
    }

    [HttpGet("{idx}")]
    public async Task<ActionResult> GetUserDetails([FromRoute] int idx)
    {
        var result = await _user.GetUserDetails(idx);
        
        if (result.isSuccess == false)
        {
            Console.WriteLine("get user details fail");
            return StatusCode(StatusCodes.Status404NotFound, null);
        }

        return StatusCode(StatusCodes.Status200OK, result.details);;           
    }

    [HttpPost]
    public async Task<ActionResult> PostUser([FromBody] AddUserHttpRequest model)
    {
        var result = await _user.AddUser(model.ToAddUserRequest());

        if (result.isSuccess == false)
        {
            // TODO: 입력할 데이터에 대한 유효성 검사로 인해 반환되는 에러는 400 Error로 반환
            // TODO: try-catch로 반환되는 에러는 500 Error로 반환
            Console.WriteLine("post fail");
            return StatusCode(StatusCodes.Status404NotFound);
        }

        return StatusCode(StatusCodes.Status201Created);
    }

    [HttpPut("{idx}")]
    public async Task<ActionResult> PutUser([FromRoute] int idx, [FromForm] UpdateUserParameterModel model)
    {
        var result = await _user.UpdateUser(
            idx,
            model.UserId,
            model.UserPw,
            model.LifeBestScore
        );
        
        if (result.isSuccess == false)
        {
            Console.WriteLine("update user details fail");
            return StatusCode(StatusCodes.Status404NotFound);
        }

        var result2 = await _user.GetUserDetails(idx);
        
        if (result2.isSuccess == false)
        {
            Console.WriteLine("update user details fail");
            return StatusCode(StatusCodes.Status404NotFound);
        }
        
        return StatusCode(StatusCodes.Status200OK, result2.details);
    }

    [HttpDelete("{idx}")]
    public async Task<ActionResult> DeleteUser([FromRoute] int idx)
    {
        var result = await _user.DeleteUser(idx);
        
        if (result.isSuccess == false)
        {
            Console.WriteLine("user delete fail");
            return StatusCode(StatusCodes.Status404NotFound);
        }
        
        return StatusCode(StatusCodes.Status200OK);
    }
}
