namespace Infrastructure.Models.Response
{
    public class UserDetailsResponse
    {
        public int Idx { get; set; }
    
        // TODO: [20221106-권용진] 17번 (해결)
        // TODO: [20221106-권용진] 응답 클래스에서는 UserId, UserPw와 같이 기본값 설정을 별도로 하지않아야합니다. 
        public string UserId { get; set; }

        public string UserPw { get; set; }

        public int? LifeBestScore { get; set; }
    
        public DateTime Created { get; set; }
        
        public DateTime Updated { get; set; }
        public bool Deleted { get; set; }
    }
}