using System.ComponentModel.DataAnnotations;
using Infrastructure.Models.User;

namespace Application.Models.User;

public class UpdateUserHttpRequest
{
    [Required] public string Username { get; set; } = "";

    [Required] public string Password { get; set; } = "";

    public UpdateUserParameterRequest ToUpdateUserHttpRequest(int id)
    {
        return new UpdateUserParameterRequest()
        {
            UserId = id,
            Username = Username,
            Password = Password,
        };
    }
}