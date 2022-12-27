namespace Infrastructure.Models.Statistics;

public class GetBestScoreByCourseResponse
{
    public List<GetBestScoreByCourseItem> CourseBestScoreList { get; set; }
}

public class GetBestScoreByCourseItem
{
    public int CourseId { get; set; }
    public int BestScore { get; set; }
}


