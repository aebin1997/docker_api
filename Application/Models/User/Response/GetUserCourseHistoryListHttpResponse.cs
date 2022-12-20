using Infrastructure.Models.User;
namespace Application.Models.User;

public class GetUserCourseHistoryListHttpResponse
{
    public List<UserCourseHistoryListItem> List { get; set; }

    public GetUserCourseHistoryListHttpResponse(in List<UserCourseHistoryListItem> list)
    {
        List = list;
    }
}

