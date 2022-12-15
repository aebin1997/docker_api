using Infrastructure.Models.Course;

namespace Application.Models.Course;

public class GetLongestListByCourseHttpResponse
{
    public List<GetLongestListItem> List { get; set; }

    public GetLongestListByCourseHttpResponse(List<GetLongestListItem> list)
    {
        List = list;
    }
}