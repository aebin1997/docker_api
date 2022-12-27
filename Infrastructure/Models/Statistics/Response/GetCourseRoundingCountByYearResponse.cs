using Newtonsoft.Json.Linq;

namespace Infrastructure.Models.Statistics;

public class GetCourseRoundingCountByYearResponse
{
    public List<GetCourseRoundingCountByYearListItem> RoundingCountList { get; set; }
}

public class GetCourseRoundingCountByYearListItem
{
    public int CourseId { get; set; }
    public Dictionary<int, int> Count { get; set; }
}