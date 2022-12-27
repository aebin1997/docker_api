using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models.Statistics;

public class GetUserLongestRangeByCourseRequest
{
    public int Page { get; set; }
    
    public int PageSize { get; set; }
    
    [Required]
    public int UserId { get; set; }
    
    public int[] CourseId { get; set; }
}