// using Microsoft.AspNetCore.Mvc;
// using Infrastructure.Services;
// using Application.Models.User;
//
// namespace Application.Controllers;
//
// [Route("[controller]")]
// [ApiController]
// public class UserController : ControllerBase
// {
//     // Log
//     private readonly ILogger<UserController> _logger;
//
//     // Service 
//     private readonly IUserService _user;
//
//     public UserController(ILogger<UserController> logger, IUserService user)
//     {
//         _logger = logger;
//     
//         _user = user;
//     }
//
//     [HttpGet]
//     public async Task<ActionResult> GetUsers([FromQuery] GetUsersHttpRequest model)
//     {
//         model.Page = model.Page == 0 ? 1 : model.Page;
//         
//         var result = await _user.GetUsers(model.ToGetUsersRequest());
//
//         if (result.isSuccess == false)
//         {
//             var badRequestErrorCode = new int[] { 1000 };
//             var serverErrorCode = new int[] { 1001 }; 
//             
//             if (badRequestErrorCode.Contains(result.errorCode))
//             {
//                 return StatusCode(StatusCodes.Status400BadRequest);
//             }
//             else if (serverErrorCode.Contains(result.errorCode))
//             {
//                 return StatusCode(StatusCodes.Status500InternalServerError);
//             }
//             else
//             {
//                 _logger.LogError("회원 목록 조회에서 정의되지 않은 에러코드가 반환됨"); 
//
//                 return StatusCode(StatusCodes.Status500InternalServerError);
//             }
//         }
//         
//         var responseModel = new UserListHttpResponse(result.response);
//         
//         return StatusCode(StatusCodes.Status200OK, responseModel);
//     }
//     
//     [HttpGet("{id}")]
//     public async Task<ActionResult> GetUserDetails([FromRoute] int id)
//     {
//         var result = await _user.GetUserDetails(id);
//         
//         if (result.isSuccess == false)
//         { 
//             var badRequestErrorCode = new int[] { 1002 };
//             var serverErrorCode = new int[] { 1003, 1004 }; 
//             
//             if (badRequestErrorCode.Contains(result.errorCode))
//             {
//                 return StatusCode(StatusCodes.Status400BadRequest);
//             }
//             else if (serverErrorCode.Contains(result.errorCode))
//             {
//                 return StatusCode(StatusCodes.Status500InternalServerError);
//             }
//             else
//             {
//                 // using (LogContext.PushProperty("LogProperty", new
//                 //        {
//                 //            idx = JObject.FromObject(idx)
//                 //        }))
//                 // {
//                 //     _logger.LogError("회원 상세 조회에서 정의되지 않은 에러코드가 반환됨"); 
//                 // }
//
//                 _logger.LogError("회원 상세 조회에서 정의되지 않은 에러코드가 반환됨"); 
//                 
//                 return StatusCode(StatusCodes.Status500InternalServerError);
//             }
//         }
//
//         var responseModel = new UserDetailsHttpResponse(result.details);
//         return StatusCode(StatusCodes.Status200OK, responseModel);          
//     }
//     
//     [HttpPost]
//     public async Task<ActionResult> PostUser([FromBody] AddUserHttpRequest model)
//     {
//         var result = await _user.AddUser(model.ToAddUserRequest());
//
//         if (result.isSuccess == false)
//         {
//             var badRequestErrorCode = new int[] { 1005, 1006 };
//             var serverErrorCode = new int[] { 1007 }; 
//             
//             if (badRequestErrorCode.Contains(result.errorCode))
//             {
//                 return StatusCode(StatusCodes.Status400BadRequest);
//             }
//             else if (serverErrorCode.Contains(result.errorCode))
//             {
//                 return StatusCode(StatusCodes.Status500InternalServerError);
//             }
//             else
//             {
//                 // using (LogContext.PushProperty("LogProperty", new
//                 //        {
//                 //            model = JObject.FromObject(model)
//                 //        }))
//                 // {
//                 //     _logger.LogError("회원 추가에 대한 결과로 정의되지 않은 에러코드가 반환됨"); 
//                 // }
//                 
//                 _logger.LogError("회원 추가에 대한 결과로 정의되지 않은 에러코드가 반환됨"); 
//                 
//                 return StatusCode(StatusCodes.Status500InternalServerError);
//             }
//         }
//
//         return StatusCode(StatusCodes.Status201Created);
//     }
//
//     [HttpPut("{id}")]
//     public async Task<ActionResult> PutUser([FromRoute] int id, [FromBody] UpdateUserHttpRequest model)
//     {
//         var result = await _user.UpdateUser(model.ToUpdateUserHttpRequest(id));
//
//         if (result.isSuccess == false)
//         {
//             var badRequestErrorCode = new int[] { 1008, 1010, 1011 };
//             var serverErrorCode = new int[] { 1009, 1012 }; 
//             if (badRequestErrorCode.Contains(result.errorCode))
//             {
//                 return StatusCode(StatusCodes.Status400BadRequest);
//             }
//             else if (serverErrorCode.Contains(result.errorCode))
//             {
//                 return StatusCode(StatusCodes.Status500InternalServerError);
//             }
//             else
//             {
//                 // using (LogContext.PushProperty("LogProperty", new
//                 //        {
//                 //            idx = JObject.FromObject(idx)
//                 //        }))
//                 // {
//                 //     _logger.LogError("회원 정보 수정중 정의되지 않은 에러코드가 반환됨"); 
//                 // }
//
//                 _logger.LogError("회원 정보 수정중 정의되지 않은 에러코드가 반환됨"); 
//                 
//                 return StatusCode(StatusCodes.Status500InternalServerError);
//             }
//         }
//
//         var result2 = await _user.GetUserDetails(id);
//
//         if (result2.isSuccess == false)
//         {
//             return StatusCode(StatusCodes.Status500InternalServerError);
//         }
//
//         var responseModel = new UserDetailsHttpResponse(result2.details);
//         return StatusCode(StatusCodes.Status200OK, responseModel);
//     }
//
//     [HttpDelete("{id}")]
//     public async Task<ActionResult> DeleteUser([FromRoute] int id)
//     {
//         var result = await _user.DeleteUser(id);
//         
//         if (result.isSuccess == false)
//         {
//             var badRequestErrorCode = new int[] { 1013 };
//             var serverErrorCode = new int[] { 1014, 1015 }; 
//             if (badRequestErrorCode.Contains(result.errorCode))
//             {
//                 return StatusCode(StatusCodes.Status400BadRequest);
//             }
//             else if (serverErrorCode.Contains(result.errorCode))
//             {
//                 return StatusCode(StatusCodes.Status500InternalServerError);
//             }
//             else
//             {
//                 // using (LogContext.PushProperty("LogProperty", new
//                 //        {
//                 //            idx = JObject.FromObject(idx)
//                 //        }))
//                 // {
//                 //     _logger.LogError("회원 삭제중 정의되지 않은 에러코드가 반환됨"); 
//                 // }
//
//                 _logger.LogError("회원 삭제중 정의되지 않은 에러코드가 반환됨"); 
//                 
//                 return StatusCode(StatusCodes.Status500InternalServerError);
//             }
//         }
//         
//         return StatusCode(StatusCodes.Status200OK);
//     }
// }
