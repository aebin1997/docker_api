namespace Infrastructure.Models.User
{
    public class UserDetailsResponse
    {
        public int UserId { get; set; }
    
        public string Username { get; set; }

        public string Password { get; set; }
        
        public string Name { get; set; }

        public ulong Created { get; set; }
        
        public ulong Updated { get; set; }
        public bool Deleted { get; set; }
    }
}