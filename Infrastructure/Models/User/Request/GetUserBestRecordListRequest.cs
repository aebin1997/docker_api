namespace Infrastructure.Models.User;

public class GetUserBestRecordListRequest
{
    public int Page { get; set; }
    
    public int PageSize { get; set; }
    
    public int[] UserId { get; set; }
    
    public int? BestRecordType { get; set; }
    
    public int? BestRecordRangeStart { get; set; }
    
    public int? BestRecordRangeEnd { get; set; }
}