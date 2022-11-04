using System.ComponentModel.DataAnnotations;
using Infrastructure.Models.Request;

namespace Application.Models.User.Request;

public class GetUsersHttpRequest
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

    public GetUsers ToGetUsersRequest()
    {
        return new GetUsers()
        {
            Page = Page,
            PageSize = PageSize,
            StartLifeBestScore = StartLifeBestScore,
            EndLifeBestScore = EndLifeBestScore
        };
    }
}