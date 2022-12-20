using Infrastructure.Models.User;
namespace Application.Models.User;

public class GetUserClubInfoListHttpResponse
{
    public List<UserClubInfoListItem> List { get; set; }

    public GetUserClubInfoListHttpResponse(in List<UserClubInfoListItem> list)
    {
        List = list;
    }
}