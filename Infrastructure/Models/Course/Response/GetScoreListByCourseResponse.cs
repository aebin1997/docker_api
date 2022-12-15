namespace Infrastructure.Models.Course;

public class GetScoreListByCourseResponse
{
    public List<GetScoreListItem> List { get; set; }
}

public class GetScoreListItem
{
    public int CourseId { get; set; }
    public List<GetScoreItem> List { get; set; }
}

public class GetScoreItem
{
    public int Score { get; set; }
}