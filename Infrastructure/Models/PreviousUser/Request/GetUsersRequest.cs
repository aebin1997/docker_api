using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models.User;

public class GetUsersRequest
{
    public int Page { get; set; }

    public int PageSize { get; set; }

}