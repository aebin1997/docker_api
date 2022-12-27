using Infrastructure.Models.Statistics;
namespace Application.Models.Statistics;

public class GetUserScoreRangeByCourseHttpResponse
{
    public int UserId { get; set; }
    
    public List<GetUserScoreRangeByCourseItem> CourseScoreRangeList { get; set; }

    public GetUserScoreRangeByCourseHttpResponse(GetUserScoreRangeByCourseResponse response)
    {
        UserId = response.UserId;
        CourseScoreRangeList = response.CourseScoreRangeList;
    }
}