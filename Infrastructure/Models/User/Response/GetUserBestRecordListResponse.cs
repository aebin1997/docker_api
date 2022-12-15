namespace Infrastructure.Models.User;

public class GetUserBestRecordListResponse
{
    public List<UserBestRecordListItem> List { get; set; }
}

public class UserBestRecordListItem
{
    public string Name { get; set; }
    
    public int Score { get; set; }
    
    public decimal Longest { get; set; }
}
