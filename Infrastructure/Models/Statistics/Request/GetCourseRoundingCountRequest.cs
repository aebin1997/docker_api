namespace Infrastructure.Models.Statistics;

public class GetCourseRoundingCountRequest
{
    public int Page { get; set; }
    
    public int PageSize { get; set; }
    
    public int[] CourseId { get; set; }
}