namespace Infrastructure.Models.Response;

public class UserListResponse
{
    public int TotalCount { get; set; }

    public List<UserListItem> List { get; set; }
}

public class UserListItem
{
    public int Idx { get; set; }

    public string UserId { get; set; } = "";
        
    public int? LifeBestScore { get; set; }
        
    public DateTime Created { get; set; }
        
    public DateTime Updated { get; set; }
        
    public bool Deleted { get; set; }
}