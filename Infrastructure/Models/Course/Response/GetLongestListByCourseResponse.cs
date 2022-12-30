namespace Infrastructure.Models.Course;

public class GetLongestListByCourseResponse
{
    public List<GetLongestListItem> List { get; set; }
}

public class GetLongestListItem
{
    public int RoundId { get; set; }
    public int CourseId { get; set; }
    public decimal Longest { get; set; }
}