using Infrastructure.Models.Response;

namespace Application.Models.User.Response;

public class UserDetailsHttpResponse
{
    public int Idx { get; set; }
    
    public string UserId { get; set; } = "";

    public string UserPw { get; set; } = "";
    
    public int? LifeBestScore { get; set; }
    
    public DateTime Created { get; set; }
        
    public DateTime Updated { get; set; }
        
    public bool Deleted { get; set; }

    public UserDetailsHttpResponse(UserDetailsResponse response)
    {
        Idx = response.Idx;
        UserId = response.UserId;
        UserPw = response.UserPw;
        LifeBestScore = response.LifeBestScore;
        Created = response.Created;
        Updated = response.Updated;
        Deleted = response.Deleted;
    }
}