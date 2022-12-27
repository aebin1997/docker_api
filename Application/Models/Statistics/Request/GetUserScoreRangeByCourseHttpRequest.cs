#nullable enable
using System.ComponentModel.DataAnnotations;
using Infrastructure.Models.Statistics;

namespace Application.Models.Statistics;

public class GetUserScoreRangeByCourseHttpRequest
{
    [Required]
    public int Page { get; set; }
    
    [Required]
    public int PageSize { get; set; }
    
    public int[] CourseId { get; set; }

    public GetUserScoreRangeByCourseRequest ToGetUserScoreRangeByCourse(int userId)
    {
        var result = new  GetUserScoreRangeByCourseRequest()
        {
            Page = Page,
            PageSize = PageSize,
            UserId = userId,
            CourseId = CourseId
        };

        return result;
    } 
}