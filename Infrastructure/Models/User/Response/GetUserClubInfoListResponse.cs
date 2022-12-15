namespace Infrastructure.Models.User;

public class GetUserClubInfoListResponse
{
    public List<UserClubInfoListItem> List { get; set; }
}


public class UserClubInfoListItem
{
    public int UserId { get; set; }

    public List<UserClubInfoItem> List { get; set; }
} 

public class UserClubInfoItem
{
    public string Club { get; set; }

    public decimal Distance { get; set; }
}