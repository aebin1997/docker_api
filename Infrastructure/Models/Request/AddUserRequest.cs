using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models.Request;

public class AddUserRequest
{
    public string UserId { get; set; } = "";

    public string UserPw { get; set; } = "";

    [Range(0, 150)] 
    public int? LifeBestScore { get; set; }
}