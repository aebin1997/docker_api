using Infrastructure.Models.Statistics;
namespace Application.Models.Statistics;

public class GetUserScoreRangeByCourseHttpResponse
{
    public int A { get; set; }
    public int B { get; set; }
    public int C { get; set; }
    public int D { get; set; }
    public int E { get; set; }
    public int F { get; set; }
    public int G { get; set; }
    public int H { get; set; }
    public int I { get; set; }

    public GetUserScoreRangeByCourseHttpResponse(GetUserScoreRangeByCourseResponse response)
    {
        A = response.A;
        B = response.B;
        C = response.C;
        D = response.D;
        E = response.E;
        F = response.F;
        G = response.G;
        H = response.H;
        I = response.I;
    }
}