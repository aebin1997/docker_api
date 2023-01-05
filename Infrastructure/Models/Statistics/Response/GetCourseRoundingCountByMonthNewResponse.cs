using Newtonsoft.Json.Linq;

namespace Infrastructure.Models.Statistics;

public class GetCourseRoundingCountByMonthNewResponse
{ 
    public List<GetCourseRoundingCountByMonthNewListItem> RoundingCountList { get; set; }

}

public class GetCourseRoundingCountByMonthNewListItem
{
    public int CourseId { get; set; }

    public JObject Count1 { get; set; }
    public Dictionary<int, Dictionary<int, int>> Count2 { get; set; }
    
    public Dictionary<int, Dictionary<int, int>> Count3 { get; set; }
}