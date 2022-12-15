namespace Infrastructure.Models.Statistics;

public class GetCourseRoundingCountByYearResponse
{
    public int CourseId { get; set; }
    
    public List<GetCourseRoundingCountByYearListItem> List { get; set; }
}

public class GetCourseRoundingCountByYearListItem
{
    public int Year { get; set; }
    public int Count { get; set; }
}