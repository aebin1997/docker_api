using System.ComponentModel.DataAnnotations;
using Infrastructure.Models.Statistics;

namespace Application.Models.Statistics;

public class GetUserLongestRangeByCourseHttpRequest
{
    [Required]
    public int Page { get; set; }
    
    [Required]
    public int PageSize { get; set; }
    
    public int[]? CourseId { get; set; }

    public GetUserLongestRangeByCourseRequest ToGetUserLongestRangeByCourse(int userId)
    {
        var result = new  GetUserLongestRangeByCourseRequest()
        {
            Page = Page,
            PageSize = PageSize,
            UserId = userId,
            CourseId = CourseId
        };

        return result;
    }
}