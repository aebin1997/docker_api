namespace Infrastructure.Models.Statistics;

public class GetBestScoreByCourseResponse
{
    public List<GetBestScoreByCourseItem> List { get; set; }
}

public class GetBestScoreByCourseItem
{
    public int CourseId { get; set; }
    public int MaxScore { get; set; }
}

