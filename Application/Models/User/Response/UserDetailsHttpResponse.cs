using Infrastructure.Models.Response;

namespace Application.Models.User.Response;

public class UserDetailsHttpResponse
{
    public int Idx { get; set; }
    
    // TODO: [20221106-권용진] 16번
    // TODO: [20221106-권용진] UserDetailsResponse와 동일한 이슈입니다.
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