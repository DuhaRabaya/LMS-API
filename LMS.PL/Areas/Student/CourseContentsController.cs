using LMS.BLL.Services.CourseContentServices;
using LMS.DAL.DTO.Request.CourseContentRequests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LMS.PL.Areas.Student
{
    [Route("api/student/[controller]")]
    [ApiController]
    [Authorize(Roles ="Student")]
    public class CourseContentsController : ControllerBase
    {
        private readonly ICourseContentService _courseContentService;

        public CourseContentsController(ICourseContentService courseContentService)
        {
            _courseContentService = courseContentService;
        }
       
        [HttpGet("{courseId}")]
        public async Task<IActionResult> GetAllCourseContents([FromRoute] int courseId, [FromQuery] string lang = "en")
        {
            var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _courseContentService.GetCourseContentsForStudent(courseId, studentId, lang);
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }
        [HttpGet("content/{contentId}")]
        public async Task<IActionResult> GetContent([FromRoute] int contentId, [FromQuery] string lang = "en")
        {
            var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _courseContentService.GetContentForStudent(contentId, studentId , lang);
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }
        [Authorize(Roles = "Student,Instructor")]
        [HttpGet("next/{contentId}")]
        public async Task<IActionResult> GetNextContent(int contentId, [FromQuery] string lang = "en")
        {
            var response = await _courseContentService.GetNextContent(contentId, lang);
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }
        [Authorize(Roles = "Student,Instructor")]
        [HttpGet("previous/{contentId}")]
        public async Task<IActionResult> GetPreviousContent(int contentId, [FromQuery] string lang = "en")
        {
            var response = await _courseContentService.GetPreviousContent(contentId, lang);
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }

    }
}
