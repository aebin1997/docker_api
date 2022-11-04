using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models.Request;

public class GetUsers
{
    [Required]
    public int Page { get; set; }

    [Required]
    [Range(2, 10)] 
    public int PageSize { get; set; }

    [Range(0, 150)] 
    public int StartLifeBestScore { get; set; }

    [Range(0, 150)] 
    public int EndLifeBestScore { get; set; }
   }