namespace Infrastructure.Models.Statistics;

public class GetCourseRoundingCountByMonthResponse
{
    public List<GetCourseRoundingCountByMonthListItem> RoundingCountList { get; set; }
}

public class GetCourseRoundingCountByMonthListItem
{
    public int CourseId { get; set; }
    
    public Dictionary<int, Dictionary<int, int>> Count { get; set; }
}
