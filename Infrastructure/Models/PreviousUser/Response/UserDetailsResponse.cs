namespace Infrastructure.Models.User
{
    public class UserDetailsResponse
    {
        public int UserId { get; set; }
    
        // TODO: [20221106-권용진] 17번 (해결)
        // TODO: [20221106-권용진] 응답 클래스에서는 UserId, UserPw와 같이 기본값 설정을 별도로 하지않아야합니다. 
        public string Username { get; set; }

        public string Password { get; set; }
        
        public string Name { get; set; }

        public ulong Created { get; set; }
        
        public ulong Updated { get; set; }
        public bool Deleted { get; set; }
    }
}