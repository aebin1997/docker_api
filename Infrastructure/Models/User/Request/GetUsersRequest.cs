namespace Infrastructure.Models.User;

public class GetUsersRequest
{
    public int Page { get; set; }

    public int PageSize { get; set; }

    public int UserId { get; set; }
    
    public string Club { get; set; }
}