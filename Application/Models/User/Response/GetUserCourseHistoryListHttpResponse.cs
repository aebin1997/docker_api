using Infrastructure.Models.User;
namespace Application.Models.User;

public class GetUserCourseHistoryListHttpResponse
{
    public List<UserCourseHistoryListItem> List { get; set; }

    public GetUserCourseHistoryListHttpResponse(List<UserCourseHistoryListItem> list)
    {
        List = list;
    }
}

