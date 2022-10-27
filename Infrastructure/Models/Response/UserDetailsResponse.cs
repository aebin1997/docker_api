namespace Infrastructure.Models.Response
{
    public class UserDetailsResponse
    {
        public int Idx { get; set; }
    
        public string UserId { get; set; } = "";

        public string UserPw { get; set; } = "";

        public int? LifeBestScore { get; set; }
    
        public DateTime Created { get; set; }
        
        public DateTime Updated { get; set; }
        public bool Deleted { get; set; }
    }
}