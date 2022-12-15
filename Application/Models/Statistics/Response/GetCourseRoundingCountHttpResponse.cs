using Infrastructure.Models.Statistics;
namespace Application.Models.Statistics;

public class GetCourseRoundingCountHttpResponse
{
    public int CourseId { get; set; }
    
    public int Count { get; set; }

    public GetCourseRoundingCountHttpResponse(GetCourseRoundingCountResponse response)
    {
        CourseId = response.CourseId;
        Count = response.Count;
    }
}