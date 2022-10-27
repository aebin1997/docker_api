namespace Infrastructure.Models;

public class AddUserRequest
{
    public string UserId { get; set; } = "";

    public string UserPw { get; set; } = "";

    public int? LifeBestScore { get; set; }
}