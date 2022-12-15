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
//             // TODO: [박예빈] db 조회 실패를 500으로 두고 따로 표기해야하는지 (해결)
//             // TODO: [20221106-권용진] 서비스 메서드에서 반환되는 에러코드는 고유한 코드입니다. DB 조회 실패를 특정해서 처리하는게 아닙니다.
//             // TODO: [20221106-권용진] 현재 작성된 코드에선 에러코드가 500으로 반환되어 배열에 500만 들어가 있지만 서버에 대한 오류로 인해 넘어오는 에러코드가 여러개일 경우 해당 배열에는 에러코드가 여러개가 들어가 있습니다.
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
//     // TODO: [20221106-권용진] 11번 (답변 완료)
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
//         // TODO: [20221106-권용진] 15번 (답변 완료)
//         // TODO: [20221106-권용진] 회원 수정 완료 후 응답에 대한 회원 데이터를 별도로 조회하여 반환한 이유가 무엇인가요 ? - 그냥 제가 수정 잘됐는지 확인하려고 했습니다.
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
