using System.ComponentModel.DataAnnotations;
using Infrastructure.Models.Course;

namespace Application.Models.Course;

public class GetScoreListByCourseHttpRequest
{
    [Required]
    public int Page { get; set; }
    
    [Required]
    public int PageSize { get; set; }
    
    public int? CourseId { get; set; }
    
    public int? ScoreRangeStart { get; set; }
    
    public int? ScoreRangeEnd { get; set; }

    public GetScoreListByCourseRequest ToGetScoreListByCourse()
    {
        var result = new GetScoreListByCourseRequest()
        {
            Page = Page,
            PageSize = PageSize,
            CourseId = CourseId,
            ScoreRangeStart = ScoreRangeStart,
            ScoreRangeEnd = ScoreRangeEnd
        };

        return result;
    }
}