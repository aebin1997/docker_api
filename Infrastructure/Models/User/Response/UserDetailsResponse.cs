namespace Infrastructure.Models.User
{
    public class UserDetailsResponse
    {
        public int UserId { get; set; }
    
        public string Username { get; set; }

        public string Password { get; set; }
        
        public string Name { get; set; }

        public DateTime Created { get; set; }
        
        public DateTime Updated { get; set; }
        public bool Deleted { get; set; }
    }
}