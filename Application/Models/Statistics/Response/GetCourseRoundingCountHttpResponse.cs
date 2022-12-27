using Infrastructure.Models.Statistics;
namespace Application.Models.Statistics;

public class GetCourseRoundingCountHttpResponse
{
    public List<GetCourseRoundingCountItem> RoundingCountList { get; set; }    

    public GetCourseRoundingCountHttpResponse(GetCourseRoundingCountResponse response)
    {
        RoundingCountList = response.RoundingCountList;
    }
}