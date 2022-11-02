using System.ComponentModel.DataAnnotations;
using Infrastructure.Models.Request;

namespace Application.Models.User.Request;

public class AddUserHttpRequest
{
    [Required]
    public string UserId { get; set; } = "";

    [Required]
    public string UserPw { get; set; } = "";

    public int? LifeBestScore { get; set; }

    public AddUserRequest ToAddUserHttpRequest()
    {
        return new AddUserRequest()
        {
            UserId = UserId,
            UserPw = UserPw,
            LifeBestScore = LifeBestScore
        };
    }
}