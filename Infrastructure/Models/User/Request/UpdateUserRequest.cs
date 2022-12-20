namespace Infrastructure.Models.User
{
    public class UpdateUserRequest
    {
        public string ColumnName { get; set; }
    
        public string DataBefore { get; set; }
    
        public string DataAfter { get; set; }
    }

    public class UpdateUserParameterRequest
    {
        public int UserId { get; set; }
        public string Username { get; set; } 
        
        public string Password { get; set; } 
       
        public string Name { get; set; }
    }
}