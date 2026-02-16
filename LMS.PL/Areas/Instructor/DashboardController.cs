using LMS.BLL.Services.DashboardServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LMS.PL.Areas.Instructor
{
    [Route("api/instructor/[controller]")]
    [ApiController]
    [Authorize(Roles ="Instructor")]
    public class DashboardController : ControllerBase
    {
        private readonly IInstructorDashboardService _instructorDashboardService;

        public DashboardController(IInstructorDashboardService instructorDashboardService)
        {
            _instructorDashboardService = instructorDashboardService;
        }
        [HttpGet("")]
        public async Task<IActionResult> GetActiveCourseTasks([FromQuery] string lang = "en")
        {
            var instructorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _instructorDashboardService.GetInstructorDashboard(instructorId, lang);
            return Ok(response);
        }
    }
}
