using LMS.BLL.Services.DashboardServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LMS.PL.Areas.Student
{
    [Route("api/student/[controller]")]
    [ApiController]
    [Authorize(Roles ="Student")]
    public class DashboardController : ControllerBase
    {
        private readonly IStudentDashboardService _studentDashboardService;

        public DashboardController(IStudentDashboardService studentDashboardService)
        {
            _studentDashboardService = studentDashboardService;
        }
        [HttpGet("")]
        public async Task<IActionResult> GetActiveCourseTasks([FromQuery] string lang = "en")
        {
            var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _studentDashboardService.GetStudentDashboard(studentId,lang);
            return Ok(response);
        }
    }
}
