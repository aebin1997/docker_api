namespace Infrastructure.Models.Request;

public class UpdateUserRequest
{
    public int Idx { get; set; }
    
    public string UserId { get; set; } = "";

    public string UserPw { get; set; } = "";

    public int LifeBestScore { get; set; }
}