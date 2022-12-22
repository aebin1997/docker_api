namespace Infrastructure.Models.Course;

public class GetLongestListByCourseResponse
{
    public List<GetLongestListItem> List { get; set; }
}

public class GetLongestListItem
{
    public int CourseId { get; set; }
    public List<GetLongestItem> List { get; set; }
}

public class GetLongestItem
{
    public int Id { get; set; }

    public decimal Longest { get; set; }
}