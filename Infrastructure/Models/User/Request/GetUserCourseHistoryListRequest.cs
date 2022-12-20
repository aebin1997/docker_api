namespace Infrastructure.Models.User;

public class GetUserCourseHistoryListRequest
{
    public int Page { get; set; }
    
    public int PageSize { get; set; }
    
    public int? UserId { get; set; }
    
    public int[] CourseId { get; set; }
}