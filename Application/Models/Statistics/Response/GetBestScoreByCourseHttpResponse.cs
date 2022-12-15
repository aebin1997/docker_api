using Infrastructure.Models.Statistics;
namespace Application.Models.Statistics;

public class GetBestScoreByCourseHttpResponse
{
    public List<GetBestScoreByCourseItem> List { get; set; }
    
    public GetBestScoreByCourseHttpResponse(List<GetBestScoreByCourseItem> list)
    {
        List = list;
    } 
}