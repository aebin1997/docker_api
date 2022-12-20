using Infrastructure.Models.User;
namespace Application.Models.User;

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

