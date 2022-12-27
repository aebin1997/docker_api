using Infrastructure.Models.Statistics;
namespace Application.Models.Statistics;

public class GetUserLongestRangeByCourseHttpResponse
{
    public int UserId { get; set; }
    
    public List<GetUserLongestRangeByCourseItem> CourseLongestRangeList { get; set; }
    
    public GetUserLongestRangeByCourseHttpResponse(GetUserLongestRangeByCourseResponse response)
    {
        UserId = response.UserId;
        CourseLongestRangeList = response.CourseLongestRangeList;
    }
}