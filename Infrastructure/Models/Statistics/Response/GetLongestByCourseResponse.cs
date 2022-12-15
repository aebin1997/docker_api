namespace Infrastructure.Models.Statistics;

public class GetLongestByCourseResponse
{
    public List<GetLongestByCourseItem> List { get; set; }
}

public class GetLongestByCourseItem
{
    public int CourseId { get; set; }
    public decimal Longest { get; set; }
}
