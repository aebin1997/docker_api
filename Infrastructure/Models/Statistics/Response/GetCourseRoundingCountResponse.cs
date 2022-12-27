namespace Infrastructure.Models.Statistics;

public class GetCourseRoundingCountResponse
{
    public List<GetCourseRoundingCountItem> RoundingCountList { get; set; }
}

public class GetCourseRoundingCountItem
{
    public int CourseId { get; set; }
    
    public int Count { get; set; }
}