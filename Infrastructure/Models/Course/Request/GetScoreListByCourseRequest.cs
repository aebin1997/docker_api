namespace Infrastructure.Models.Course;

public class GetScoreListByCourseRequest
{
    public int Page { get; set; }
    
    public int PageSize { get; set; }
    
    public int? CourseId { get; set; }
    
    public int? ScoreRangeStart { get; set; }
    
    public int? ScoreRangeEnd { get; set; }
}