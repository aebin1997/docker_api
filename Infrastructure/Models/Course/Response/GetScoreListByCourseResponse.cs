namespace Infrastructure.Models.Course;

public class GetScoreListByCourseResponse
{
    public List<GetScoreListItem> List { get; set; }
}

public class GetScoreListItem
{
    public int RoundId { get; set; }
    public int CourseId { get; set; }
    public int Score { get; set; }
}

