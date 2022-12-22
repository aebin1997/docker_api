using System.ComponentModel.DataAnnotations;
using Infrastructure.Models.Course;

namespace Application.Models.Course;

public class GetLongestListByCourseHttpRequest
{
    [Required]
    public int Page { get; set; }
    
    [Required]
    public int PageSize { get; set; }
    
    public int? CourseId { get; set; }
    
    public int? LongestRangeStart { get; set; }
    
    public int? LongestRangeEnd { get; set; }

    public GetLongestListByCourseRequest ToGetLongestListByCourse()
    {
        var result = new GetLongestListByCourseRequest()
        {
            Page = Page,
            PageSize = PageSize,
            CourseId = CourseId,
            LongestRangeStart = LongestRangeStart,
            LongestRangeEnd = LongestRangeEnd
        };

        return result;
    }
}