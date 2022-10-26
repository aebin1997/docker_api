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
            var result = _user.AddUser(
                model.UserId,
                model.UserPw,
                model.LifeBestScore
            );

            // if (result.isSuccess == false)
            // {
            //     Console.WriteLine("post fail");
            // }

            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpGet]
        public async Task<ActionResult> GetUsers([FromQuery] UserListResponse model)
        {
            var result = _user.GetUsers();

            if (result.isSuccess == false)
            {
                Console.WriteLine("get todo list fail");
            }

            UserListResponse responseModel = new UserListResponse(result.totalCount, result.list);
            
            return StatusCode(StatusCodes.Status200OK, responseModel);
        }

        [HttpGet("{idx}")]
        public async Task<ActionResult<UserModel>> GetUserDetails([FromRoute] int idx)
        {
            var result = _user.GetUserDetails(idx);
            
            if (result.isSuccess == false)
            {
                Console.WriteLine("get todo details fail");
            }

            return StatusCode(StatusCodes.Status200OK, result.details);;           
        }


        [HttpPut("delete/{idx}")]
        public async Task<ActionResult> DeleteUser([FromRoute] int idx)
        {
            var result = _user.DeleteUser(idx);
            
            // if (result.isSu == false)
            // {
            //     Console.WriteLine("delete fail");
            // }
            
            return StatusCode(StatusCodes.Status200OK);
        }

        [HttpPut("{idx}")]
        public async Task<ActionResult<UserModel>> UpdateUser([FromBody] UpdateUserRequest model)
        {
            var result = _user.UpdateUser(
                    model.Idx,
                    model.UserId,
                    model.UserPw,
                    model.LifeBestScore
                );
            
            if (result.isSuccess == false)
            {
                Console.WriteLine("get todo details fail");
            }

            return StatusCode(StatusCodes.Status200OK, result.details);;
        }
    }
}