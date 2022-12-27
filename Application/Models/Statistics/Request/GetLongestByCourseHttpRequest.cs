using System.ComponentModel.DataAnnotations;
using Infrastructure.Models.Statistics;

namespace Application.Models.Statistics;

public class GetLongestByCourseHttpRequest
{
    [Required]
    public int Page { get; set; }
    
    [Required]
    public int PageSize { get; set; }
    
    public int[] CourseId { get; set; }

    public GetLongestByCourseRequest ToGetLongestByCourse()
    {
        var result = new  GetLongestByCourseRequest()
        {
            Page = Page,
            PageSize = PageSize,
            CourseId = CourseId
        };

        return result;
    }
}