namespace Infrastructure.Models.Course;

public class GetLongestListByCourseRequest
{
    public int Page { get; set; }
    
    public int PageSize { get; set; }
    
    public int? CourseId { get; set; }
    
    public int? LongestRangeStart { get; set; }
    
    public int? LongestRangeEnd { get; set; }
}