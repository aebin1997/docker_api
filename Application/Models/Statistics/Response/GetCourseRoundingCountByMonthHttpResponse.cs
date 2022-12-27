using Infrastructure.Models.Statistics;
namespace Application.Models.Statistics;

public class GetCourseRoundingCountByMonthHttpResponse
{
    public List<GetCourseRoundingCountByMonthListItem> RoundingCountList { get; set; }
    
    public GetCourseRoundingCountByMonthHttpResponse(GetCourseRoundingCountByMonthResponse response)
    {
        RoundingCountList = response.RoundingCountList;
    } 
}