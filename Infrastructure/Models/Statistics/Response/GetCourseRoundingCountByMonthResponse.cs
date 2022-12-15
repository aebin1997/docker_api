namespace Infrastructure.Models.Statistics;

public class GetCourseRoundingCountByMonthResponse
{
    public int CourseId { get; set; }
    
    public List<GetCourseRoundingCountByMonthListItem> List { get; set; }
}

public class GetCourseRoundingCountByMonthListItem
{
    public int Month { get; set; }
    public int Count { get; set; }
}