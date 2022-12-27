namespace Infrastructure.Models.Statistics;

public class GetCourseRoundingCountByMonthRequest
{
    public int Page { get; set; }
    
    public int PageSize { get; set; }
    
    public int[] CourseId { get; set; }
    
    public int? YearRangeStart { get; set; }
    
    public int? YearRangeEnd { get; set; }
    
    public int? MonthRangeStart { get; set; }
    
    public int? MonthRangeEnd { get; set; }
}