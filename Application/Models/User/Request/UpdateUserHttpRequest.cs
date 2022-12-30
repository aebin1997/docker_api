#nullable enable
using Infrastructure.Models.User;

namespace Application.Models.User;

public class UpdateUserHttpRequest
{
    public string? Username { get; set; } 

    public string? Password { get; set; } 
    
    public string? Name { get; set; }

    public UpdateUserParameterRequest ToUpdateUserHttpRequest(int id)
    {
        return new UpdateUserParameterRequest()
        {
            UserId = id,
            Username = Username,
            Password = Password,
            Name = Name
        };
    }
}