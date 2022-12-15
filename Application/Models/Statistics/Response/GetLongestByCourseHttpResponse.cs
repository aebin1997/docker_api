using Infrastructure.Models.Statistics;
namespace Application.Models.Statistics;

public class GetLongestByCourseHttpResponse
{
    public List<GetLongestByCourseItem> List { get; set; }
    
    public GetLongestByCourseHttpResponse(List<GetLongestByCourseItem> list)
    {
        List = list;
    }
}


