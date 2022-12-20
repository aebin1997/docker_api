using System.ComponentModel.DataAnnotations;
using Infrastructure.Models.User;

namespace Application.Models.User.Request;

public class GetUserClubInfoListHttpRequest
{
    [Required]
    public int Page { get; set; }
    
    [Required]
    public int PageSize { get; set; }
    
    public int? UserId { get; set; }
    
    public string[] Club { get; set; }
    
    public GetUserClubInfoListRequest GetUserClubInfoList()
    {
        return new GetUserClubInfoListRequest()
        {
            Page = Page,
            PageSize = PageSize,
            UserId = UserId,
            Club = Club
        };
    }
}