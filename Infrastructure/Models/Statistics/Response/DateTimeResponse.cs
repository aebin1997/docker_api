namespace Infrastructure.Models.Statistics;

public class DateTimeResponse
{
    public int UserId { get; set; }

    public int CourseId { get; set; }
    
    public int Score { get; set; }
    
    public decimal Longest { get; set; }
    
    public int Updated { get; set; }
}