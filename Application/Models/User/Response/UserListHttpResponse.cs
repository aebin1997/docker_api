using Infrastructure.Models.Response;
namespace Application.Models.User.Response;

public class UserListHttpResponse
{
    public int TotalCount { get; set; }

    public List<UserListItem> List { get; set; } 

    public UserListHttpResponse(UserListResponse response)
    {
        TotalCount = response.TotalCount;
        List = response.List;
    }
}

