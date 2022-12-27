namespace Infrastructure.Models.Statistics;

public class GetUserScoreRangeByCourseResponse
{
    public int UserId { get; set; }
    public List<GetUserScoreRangeByCourseItem> CourseScoreRangeList { get; set; }
}

public class GetUserScoreRangeByCourseItem
{
    public int CourseId { get; set; }
    public ScoreRangeItem ScoreRange { get; set; }
}

public class ScoreRangeItem
{
    public int Score60To69 { get; set; } 
    
    public int Score70To79 { get; set; }
    
    public int Score80To89 { get; set; }
    
    public int Score90To99 { get; set; }
    
    public int Score100To109 { get; set; }
    
    public int Score110To119 { get; set; }
    
    public int Score120To129 { get; set; }
    
    public int Score130To139 { get; set; }
    
    public int Score140To144 { get; set; }
}
