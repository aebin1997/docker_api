namespace Infrastructure.Models.Request;

public class AddUserRequest
{
    public string UserId { get; set; } = "";

    public string UserPw { get; set; } = "";

    // nullable ? 달았더니 controller 에서 에러남
    public int LifeBestScore { get; set; }
}