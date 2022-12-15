using Infrastructure.Models.Statistics;
namespace Application.Models.Statistics;

public class GetUserLongestRangeByCourseHttpResponse
{
    public int A { get; set; }
    public int B { get; set; }
    public int C { get; set; }
    public int D { get; set; }
    public int E { get; set; }
    public int F { get; set; }
    
    public GetUserLongestRangeByCourseHttpResponse(GetUserLongestRangeByCourseResponse response)
    {
        A = response.A;
        B = response.B;
        C = response.C;
        D = response.D;
        E = response.E;
        F = response.F;
    }
}