using Infrastructure.Models.Statistics;
namespace Application.Models.Statistics;

public class GetLongestByCourseHttpResponse
{
    public List<GetLongestByCourseItem> CourseBestLongestList { get; set; }
    
    public GetLongestByCourseHttpResponse(GetLongestByCourseResponse response)
    {
        CourseBestLongestList = response.CourseLongestList;
    }
}


