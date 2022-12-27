using Infrastructure.Models.Statistics;
namespace Application.Models.Statistics;

// public class GetCourseRoundingCountByYearHttpResponse
// {
//     public int A { get; set; }
//     public int B { get; set; }
//     public int C { get; set; }
//     public int D { get; set; }
//     public int E { get; set; }
//
//     public GetCourseRoundingCountByYearHttpResponse(GetCourseRoundingCountByYearResponse response)
//     {
//         A = response.A;
//         B = response.B;
//         C = response.C;
//         D = response.D;
//         E = response.E;
//     }
// }

public class GetCourseRoundingCountByYearHttpResponse
{
    public List<GetCourseRoundingCountByYearListItem> RoundingCountList { get; set; }
    
    public GetCourseRoundingCountByYearHttpResponse(GetCourseRoundingCountByYearResponse response)
    {
        RoundingCountList = response.RoundingCountList;
    } 
}