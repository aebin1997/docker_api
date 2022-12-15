using System.ComponentModel.DataAnnotations;

namespace Application.Models.Course;

public class PagingHttpRequest
{
    [Required]
    public int Page { get; set; }
    
    [Required]
    [Range(1, 10)]
    public int PageSize { get; set; }
    
    [Required]
    public int CourseId { get; set; }
}