using System.ComponentModel.DataAnnotations;
using Infrastructure.Models.Statistics;

namespace Application.Models.Statistics;

public class GetBestScoreByCourseHttpRequest
{
    [Required]
    public int Page { get; set; }
    
    [Required]
    public int PageSize { get; set; }
    
    public int[]? CourseId { get; set; }

    public GetBestScoreByCourseRequest ToGetBestScoreByCourse()
    {
        var result = new  GetBestScoreByCourseRequest()
        {
            Page = Page,
            PageSize = PageSize,
            CourseId = CourseId
        };

        return result;
    }
}