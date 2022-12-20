namespace Infrastructure.Models.User;

public class GetUsersResponse
{
    /// <summary>
    /// 조회된 회원 수
    /// </summary>
    public int TotalCount { get; set; }
    
    /// <summary>
    /// 회원 목록
    /// </summary>
    public List<UserListItem> List { get; set; }
}