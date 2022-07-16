using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Skimur.Web.Controllers.Api
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AccountController : ControllerBase
    {
    }
}
