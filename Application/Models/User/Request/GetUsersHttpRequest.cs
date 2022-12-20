using System.ComponentModel.DataAnnotations;
using Infrastructure.Models.User;

namespace Application.Models.User;

public class GetUsersHttpRequest
{
    [Required]
    public int Page { get; set; }

    [Required]
    public int PageSize { get; set; }
    
    public int UserId { get; set; }
    
    public string Club { get; set; }

    public GetUsersRequest ToGetUsersRequest()
    {
        return new GetUsersRequest()
        {
            Page = Page,
            PageSize = PageSize,
            UserId = UserId,
            Club = Club
        };
    }
}