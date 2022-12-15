namespace Infrastructure.Models.User;

public class UserListResponse
{
    public int TotalCount { get; set; }

    public List<UserListItem> List { get; set; }
}

public class UserListItem
{
    public int UserId { get; set; }

    public string Username { get; set; } = "";
        
    public ulong Created { get; set; }
        
    public ulong Updated { get; set; }
        
    public bool Deleted { get; set; }
}