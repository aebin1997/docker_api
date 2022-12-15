using Infrastructure.Models.Course;

namespace Application.Models.Course;

public class GetScoreListByCourseHttpResponse
{
    public List<GetScoreListItem> List { get; set; }

    public GetScoreListByCourseHttpResponse(List<GetScoreListItem> list)
    {
        List = list;
    }
}