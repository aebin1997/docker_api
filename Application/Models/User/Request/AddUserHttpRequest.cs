using System.ComponentModel.DataAnnotations;
using Infrastructure.Models.User;

namespace Application.Models.User;

public class AddUserHttpRequest
{
    [Required]
    public string Username { get; set; } 

    [Required]
    public string Password { get; set; } 
    
    [Required]
    public string Name { get; set; }

    public AddUserRequest ToAddUserRequest()
    {
        return new AddUserRequest()
        {
            Username = Username,
            Password = Password,
            Name = Name
        };
    }
}