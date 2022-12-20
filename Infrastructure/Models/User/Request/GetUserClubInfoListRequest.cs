namespace Infrastructure.Models.User;

public class GetUserClubInfoListRequest
{
    public int Page { get; set; }
    
    public int PageSize { get; set; }
    
    public int? UserId { get; set; }
    
    public string[] Club { get; set; } 
}