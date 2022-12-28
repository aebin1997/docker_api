using System.ComponentModel.DataAnnotations;
using Infrastructure.Models.Statistics;

namespace Application.Models.Statistics;

public class GetCourseRoundingCountByYearHttpRequest
{
    [Required]
    public int Page { get; set; }
    
    [Required]
    public int PageSize { get; set; }
    
    public int[]? CourseId { get; set; }
    
    public int? YearRangeStart { get; set; }
    
    public int? YearRangeEnd { get; set; }

    public GetCourseRoundingCountByYearRequest ToGetCourseRoundingCountByYear()
    {
        var result = new  GetCourseRoundingCountByYearRequest()
        {
            Page = Page,
            PageSize = PageSize,
            CourseId = CourseId,
            YearRangeStart = YearRangeStart,
            YearRangeEnd = YearRangeEnd
        };

        return result;
    }
}