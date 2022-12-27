namespace Infrastructure.Models.Statistics;

public class GetUserScoreRangeByCourseRequest
{
    public int Page { get; set; }
    
    public int PageSize { get; set; }
    
    public int UserId { get; set; }
    
    public int[] CourseId { get; set; }

}