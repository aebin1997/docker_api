namespace Infrastructure.Models.User;

public class UserListResponse
{
    public int TotalCount { get; set; }

    public List<UserListItem> List { get; set; }
}

public class UserListItem
{
    public int UserId { get; set; }

    public string Username { get; set; } 
        
    public DateTime Created { get; set; }
        
    public DateTime Updated { get; set; }
        
    public bool Deleted { get; set; }
}