using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models.Request;

// TODO: [20221106-권용진] 2번
// TODO: [20221106-권용진] 서비스단 Model Naming의 경우 GetUsersRequest와 같이 요청의 경우 맨 뒤에 Request를, 응답의 경우 맨뒤에 Response를 추가해주세요.
public class GetUsers
{
    // TODO: [20221106-권용진] 3번
    // TODO: [20221106-권용진] Http Request에서 이미 Attribyte가 지정되어있어서 서비스단 Model에는 추가하지 않아도됩니다.
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