namespace Infrastructure.Models.Statistics;

public class GetCourseRoundingCountByYearRequest
{
    public int Page { get; set; }
    
    public int PageSize { get; set; }
    
    public int[] CourseId { get; set; }
    
    public int? YearRangeStart { get; set; }
    
    public int? YearRangeEnd { get; set; }

}