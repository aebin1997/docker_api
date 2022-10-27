using System.ComponentModel.DataAnnotations;
using Infrastructure.Models;

namespace Application.Models.User;

public class AddUserHttpRequest
{
    [Required]
    public string UserId { get; set; } = "";

    [Required]
    public string UserPw { get; set; } = "";

    public int? LifeBestScore { get; set; }

    public AddUserRequest ToAddUserRequest()
    {
        return new AddUserRequest()
        {
            UserId = UserId,
            UserPw = UserPw,
            LifeBestScore = LifeBestScore,
        };
    }
}