using System.ComponentModel.DataAnnotations;
using Infrastructure.Models.Request;

namespace Application.Models.User.Request;

public class AddUserHttpRequest
{
    [Required]
    public string UserId { get; set; } = "";

    [Required]
    public string UserPw { get; set; } = "";

    [Range(0, 150)] 
    public int? LifeBestScore { get; set; }

    public AddUserRequest ToAddUserRequest()
    {
        return new AddUserRequest()
        {
            UserId = UserId,
            UserPw = UserPw,
            LifeBestScore = LifeBestScore
        };
    }
}