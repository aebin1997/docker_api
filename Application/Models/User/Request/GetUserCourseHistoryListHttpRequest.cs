#nullable enable
using System.ComponentModel.DataAnnotations;
using Infrastructure.Models.User;

namespace Application.Models.User.Request;

public class GetUserCourseHistoryListHttpRequest
{
    [Required]
    public int Page { get; set; }
    
    [Required]
    public int PageSize { get; set; }
    
    public int? UserId { get; set; }
    
    public int[]? CourseId { get; set; }

    public GetUserCourseHistoryListRequest ToGetUserCourseHistoryList()
    {
        return new GetUserCourseHistoryListRequest()
        {
            Page = Page,
            PageSize = PageSize,
            UserId = UserId,
            CourseId = CourseId
        };
    }
}