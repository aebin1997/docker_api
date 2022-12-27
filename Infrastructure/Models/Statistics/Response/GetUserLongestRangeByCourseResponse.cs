namespace Infrastructure.Models.Statistics;

public class GetUserLongestRangeByCourseResponse
{
    public int UserId { get; set; }
    public List<GetUserLongestRangeByCourseItem> CourseLongestRangeList { get; set; }
}

public class GetUserLongestRangeByCourseItem
{
    public int CourseId { get; set; }
    public LongestRangeItem LongestRange { get; set; }
}

public class LongestRangeItem
{
    public int Longest160To179 { get; set; } 
    
    public int Longest180To199 { get; set; }
    
    public int Longest200To219 { get; set; }
    
    public int Longest220To239 { get; set; }
    
    public int Longest240To259 { get; set; }
    
    public int Longest260To279 { get; set; }
    
    public int Longest280To299 { get; set; }
}