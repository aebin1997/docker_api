using Infrastructure.Models.Statistics;
namespace Application.Models.Statistics;

public class GetCourseRoundingCountByMonthHttpResponse
{
    public int CourseId { get; set; }
    public List<GetCourseRoundingCountByMonthListItem> List { get; set; }

    public GetCourseRoundingCountByMonthHttpResponse(GetCourseRoundingCountByMonthResponse response)
    {
        CourseId = response.CourseId;
        List = response.List;
    }
}