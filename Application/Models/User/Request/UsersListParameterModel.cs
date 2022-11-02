using System.ComponentModel.DataAnnotations;

namespace Application.Models.User.Request;

public class UsersListParameterModel
{
    [Required]
    public int Page { get; set; }

    [Required]
    [Range(2, 10)]
    public int PageSize { get; set; }
}