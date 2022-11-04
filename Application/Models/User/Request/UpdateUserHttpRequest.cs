using System.ComponentModel.DataAnnotations;
using Infrastructure.Models.Request;

namespace Application.Models.User.Request;

public class UpdateUserHttpRequest
{
    [Required] public string UserId { get; set; } = "";

    [Required] public string UserPw { get; set; } = "";

    [Range(0, 150)] 
    public int? LifeBestScore { get; set; }

    public UpdateUserParameterModel ToUpdateUserHttpRequest(int idx)
    {
        return new UpdateUserParameterModel()
        {
            Idx = idx,
            UserId = UserId,
            UserPw = UserPw,
            LifeBestScore = LifeBestScore
        };
    }
}