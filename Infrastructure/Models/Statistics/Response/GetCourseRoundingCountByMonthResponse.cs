namespace Infrastructure.Models.Statistics;

public class GetCourseRoundingCountByMonthResponse
{
    public List<GetCourseRoundingCountByMonthListItem> RoundingCountList { get; set; }
}

public class GetCourseRoundingCountByMonthListItem
{
    public int CourseId { get; set; }
    
    public Dictionary<int, Dictionary<int, int>> Count { get; set; }
}

public class GetCourseRoundingCountByMonthListItem2
{
    public int CourseId { get; set; }
    
    public List<TestList> Count { get; set; }
}

public class TestList
{
    public int Year { get; set; }
    public List<MonthList> MonthList { get; set; }

    public TestList(int?year, List<MonthList> list)
    {
        Year = (int)year;
        MonthList = list;
    }
    
    public TestList()
    {
    }
}

public class MonthList
{
    public int Month { get; set; }
    public int Count { get; set; }
    
    public MonthList(int? month, int count)
    {
        Month = (int)month;
        Count = count;
    }
    
    public MonthList()
    {
    }
}