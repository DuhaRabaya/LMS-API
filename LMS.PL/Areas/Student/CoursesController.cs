using LMS.BLL.Services.CourseServices;
using LMS.BLL.Services.EnrollmentsServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LMS.PL.Areas.Student
{
    [Route("api/student/[controller]")]
    [ApiController]
    [Authorize(Roles ="Student")]
    
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;
        private readonly IEnrollmentService _enrollmentService;

        public CoursesController(ICourseService courseService, IEnrollmentService enrollmentService)
        {
            _courseService = courseService;
            _enrollmentService = enrollmentService;
        }
        [HttpGet("all")]
        public async Task<IActionResult> GetCourses([FromQuery] string lang = "en",
            [FromQuery]int page = 1, [FromQuery]int limit = 1, [FromQuery]string? search=null
            , [FromQuery] string? instructorId = null, [FromQuery]decimal? price=null,
             [FromQuery] decimal? minPrice = null,
             [FromQuery] decimal? maxPrice = null
            , [FromQuery] double? minRating = null, [FromQuery] double? maxRating = null
            , [FromQuery]string? sortBy = null,[FromQuery]bool asc = true)
        {
            var result = await _courseService.GetCoursesForStudent(lang, page,limit,search,instructorId,price,minPrice,maxPrice,minRating,maxRating,sortBy,asc);
            return Ok(result);
        }

        [HttpPost("enroll/{courseId}")]
        public async Task<IActionResult> Enroll([FromRoute]int courseId)
        {
            var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _enrollmentService.Enroll(studentId, courseId);
            return Ok(result);
        }
        [HttpGet("enrollments")]
        public async Task<IActionResult> GetEnrollments([FromQuery] string lang="en")
        {
            var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var enrollments = await _enrollmentService.GetStudentEnrollments(studentId, lang);

            return Ok(enrollments);
        }
    }
}
