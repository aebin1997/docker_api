using Infrastructure.Models.User;

namespace Application.Models.User;

public class UserDetailsHttpResponse
{
    public int UserId { get; set; }
    
    public string Username { get; set; } 

    public string Password { get; set; } 
    
    public string Name { get; set; }
    
    public DateTime Created { get; set; }
        
    public DateTime Updated { get; set; }
        
    public bool Deleted { get; set; }

    public UserDetailsHttpResponse(UserDetailsResponse response)
    {
        UserId = response.UserId;
        Username = response.Username;
        Password = response.Password;
        Name = response.Name;
        Created = response.Created;
        Updated = response.Updated;
        Deleted = response.Deleted;
    }
}