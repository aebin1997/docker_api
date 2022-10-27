using Domain.Entities;
using Infrastructure.Models.Request;
using Infrastructure.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Services;

namespace Application.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Produces("application/json")]
    
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

        [HttpPost]
        public async Task<ActionResult> PostUser([FromBody] AddUserRequest model)
        {
            var result = await _user.AddUser(
                model.UserId,
                model.UserPw,
                model.LifeBestScore
            );

            if (result.isSuccess == false)
            {
                Console.WriteLine("post fail");
                return StatusCode(StatusCodes.Status404NotFound);
            }

            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpGet]
        public async Task<ActionResult> GetUsers()
        {
            var result = await _user.GetUsers();

            if (result.isSuccess == false)
            {
                Console.WriteLine("get user list fail");
                return StatusCode(StatusCodes.Status404NotFound, null);
            }

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
    }
}