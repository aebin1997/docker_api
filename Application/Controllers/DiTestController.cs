using Infrastructure.Services.DI;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers;

[Route("di-test")]
[ApiController]
public class DiTestController : Controller
{
    private readonly IDiService _di1;
    private readonly IDiTwoService _di2;
    
    public DiTestController(IDiService di1, IDiTwoService di2)
    {
        _di1 = di1;
        _di2 = di2;
    }

    [HttpGet]
    public async Task<IActionResult> DiTest()
    {
        // 세션 시작
        await _di1.GetData();
        
        await _di2.GetData2();
        
        return Ok(); // 세션 종료
    }
}