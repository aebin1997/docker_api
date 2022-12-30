#nullable enable
using System.ComponentModel.DataAnnotations;
using Infrastructure.Models.Statistics;

namespace Application.Models.Statistics;

public class GetCourseRoundingCountByMonthHttpRequest
{
    [Required]
    public int Page { get; set; }
    
    [Required]
    public int PageSize { get; set; }
    
    public int[]? CourseId { get; set; }
    
    public int? YearRangeStart { get; set; }
    
    public int? YearRangeEnd { get; set; }

    public int? MonthRangeStart { get; set; }
    
    public int? MonthRangeEnd { get; set; }

    public GetCourseRoundingCountByMonthRequest ToGetCourseRoundingCountByMonth()
    {
        var result = new  GetCourseRoundingCountByMonthRequest()
        {
            Page = Page,
            PageSize = PageSize,
            CourseId = CourseId,
            YearRangeStart = YearRangeStart,
            YearRangeEnd = YearRangeEnd,
            MonthRangeStart = MonthRangeStart,
            MonthRangeEnd = MonthRangeEnd
        };

        return result;
    }
}