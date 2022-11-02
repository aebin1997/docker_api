using Infrastructure.Models.Response;
namespace Application.Models.User.Response;

public class UserListHttpResponse
{
    public int TotalCount { get; set; }

    public List<UserList> List { get; set; } 

    public UserListHttpResponse(int totalCount, List<UserList> list)
    {
        TotalCount = totalCount;
        List = list;
    }
        
}

