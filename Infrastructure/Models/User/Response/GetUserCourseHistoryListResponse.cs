namespace Infrastructure.Models.User;

public class GetUserCourseHistoryListResponse
{
    public List<UserCourseHistoryListItem> List { get; set; }
}

public class UserCourseHistoryListItem
{
    public int UserId { get; set; }
    
    public List<UserCourseHistoryItem> List { get; set; }
}

public class UserCourseHistoryItem
{
    public int CourseId { get; set; }
    
    public int Score { get; set; }
    
    public decimal Longest { get; set; }
}