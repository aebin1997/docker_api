using System.ComponentModel.DataAnnotations;

namespace Application.Models.User.Request;

public class GetUserCourseHistoryListHttpRequest
{
    [Required]
    public int Page { get; set; }
    
    [Required]
    [Range(1, 10)]
    public int PageSize { get; set; }
}