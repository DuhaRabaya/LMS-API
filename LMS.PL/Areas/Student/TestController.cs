using LMS.PL.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace LMS.PL.Areas.Student
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IStringLocalizer<SharedResource>  _stringLocalizer;

        public TestController(IStringLocalizer<SharedResource> stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
        }
        [HttpGet("")]
        public IActionResult Index()
        {
            return Ok(_stringLocalizer["Success"]);
        }
    }
}
