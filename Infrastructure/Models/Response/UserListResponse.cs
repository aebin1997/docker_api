using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models.Response;

public class UserListResponse
{
    public int TotalCount { get; set; }

    public List<UserList> List { get; set; }

    public UserListResponse(int totalCount, List<UserList> list)
    {
        TotalCount = totalCount;
        List = list;
    }
}

public class UserList
{
    /// <summary>
    /// 페이지 번호
    /// </summary>
    [Required]
    public int Page { get; set; }

    /// <summary>
    /// 페이지 사이즈
    /// Range 10~50
    /// </summary>
    [Required]
    [Range(3, 9)]
    public int PageSize { get; set; }
    
    public int Idx { get; set; }

    public string UserId { get; set; } = "";

    public string UserPw { get; set; } = "";
        
    public int? LifeBestScore { get; set; }
        
    public DateTime Created { get; set; }
        
    public DateTime Updated { get; set; }
        
    public bool Deleted { get; set; }
}