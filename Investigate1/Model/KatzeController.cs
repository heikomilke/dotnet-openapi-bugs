using Microsoft.AspNetCore.Mvc;

namespace Investigate1.Model;

[ApiController]
[Route("api/v1/[controller]")]
[ApiExplorerSettings(GroupName = "v1")]
public class KatzeController : ControllerBase
{
    [HttpPost("get-default-katze")]
    public ActionResult<ColorResponse> Health(ColorRequest colorRequest)
    {
        return new ColorResponse()
        {
            There = colorRequest.Here ?? Color.Green
        };
    }
}