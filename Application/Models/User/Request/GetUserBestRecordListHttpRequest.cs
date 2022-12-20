using System.ComponentModel.DataAnnotations;
using Infrastructure.Models.User;

namespace Application.Models.User;

public class GetUserBestRecordListHttpRequest
{
    [Required]
    public int Page { get; set; }
    
    [Required]
    public int PageSize { get; set; }
    
    public int[] UserId { get; set; }
    
    [Range (1,2)]
    public int? BestRecordType { get; set; }
    
    public int? BestRecordRangeStart { get; set; }
    
    public int? BestRecordRangeEnd { get; set; }

    public GetUserBestRecordListRequest GetUserBestRecordList()
    {
        var test = new GetUserBestRecordListRequest()
        {
            Page = Page,
            PageSize = PageSize,
            UserId = UserId,
            BestRecordType = BestRecordType,
            BestRecordRangeStart = BestRecordRangeStart,
            BestRecordRangeEnd = BestRecordRangeEnd
        };
        ;

        return test;
    }
}