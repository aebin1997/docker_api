using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models.User;

// TODO: [20221106-권용진] 2번 (수정 완료)
// TODO: [20221106-권용진] 서비스단 Model Naming의 경우 GetUsersRequest와 같이 요청의 경우 맨 뒤에 Request를, 응답의 경우 맨뒤에 Response를 추가해주세요.
public class GetUsersRequest
{
    public int Page { get; set; }

    public int PageSize { get; set; }

}