using System.ComponentModel.DataAnnotations;
using Infrastructure.Models.Statistics;

namespace Application.Models.Statistics;

public class GetCourseRoundingCountHttpRequest
{
    [Required]
    public int Page { get; set; }
         
    [Required]
    public int PageSize { get; set; }
    
    public int[] CourseId { get; set; }

    public GetCourseRoundingCountRequest ToGetCourseRoundingCount()
    {
        var result = new  GetCourseRoundingCountRequest()
        {
            Page = Page,
            PageSize = PageSize,
            CourseId = CourseId
        };

        return result;
    }
} 