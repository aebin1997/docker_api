using System.ComponentModel.DataAnnotations;
using Infrastructure.Models.User;

namespace Application.Models.User;

public class GetUsersHttpRequest
{
    [Required]
    public int Page { get; set; }

    [Required]
    [Range(2, 10)]
    public int PageSize { get; set; }

    public GetUsersRequest ToGetUsersRequest()
    {
        return new GetUsersRequest()
        {
            Page = Page,
            PageSize = PageSize,
        };
    }
}