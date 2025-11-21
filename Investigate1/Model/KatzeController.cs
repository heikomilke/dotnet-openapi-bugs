using Microsoft.AspNetCore.Mvc;

namespace Investigate1.Model;

[ApiController]
[Route("api/v1/[controller]")]
[ApiExplorerSettings(GroupName = "v1")]
public class KatzeController : ControllerBase
{
    [HttpPost("get-default-katze")]
    public ActionResult<RequiredKatze> Health(OptinalKatze optinalKatze)
    {
        return new RequiredKatze()
        {
            Katzenum = optinalKatze.katze ?? Katzenum.green
        };
    }
}