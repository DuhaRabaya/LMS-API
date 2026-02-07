using LMS.BLL.Services.CourseServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LMS.PL.Areas.Student
{
    [Route("api/student/[controller]")]
    [ApiController]
    [Authorize]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }
        [HttpGet("all")]
        public async Task<IActionResult> GetCourses([FromQuery] string lang = "en",
            [FromQuery]int page = 1, [FromQuery]int limit = 1)
        {
            var result = await _courseService.GetCourses(lang, page,limit);
            return Ok(result);
        }
    }
}
