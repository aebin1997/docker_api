using Infrastructure.Models.User;
namespace Application.Models.User;

public class GetUserBestRecordListHttpResponse
{
    public List<UserBestRecordListItem> List { get; set; }

    public GetUserBestRecordListHttpResponse(in List<UserBestRecordListItem> list)
    {
        List = list;
    }
}