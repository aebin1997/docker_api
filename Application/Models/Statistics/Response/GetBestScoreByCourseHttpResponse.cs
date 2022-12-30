using Infrastructure.Models.Statistics;
namespace Application.Models.Statistics;

public class GetBestScoreByCourseHttpResponse
{
    public List<GetBestScoreByCourseItem> CourseBestScoreList { get; set; }

    public GetBestScoreByCourseHttpResponse(GetBestScoreByCourseResponse response)
    {
        CourseBestScoreList = response.CourseBestScoreList;
    } 
}