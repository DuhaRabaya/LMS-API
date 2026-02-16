using LMS.BLL.Services.ContentProgressServices;
using LMS.BLL.Services.CourseContentServices;
using LMS.DAL.DTO.Request.CourseContentRequests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LMS.PL.Areas.Instructor
{
    [Route("api/instructor/[controller]")]
    [ApiController]
    [Authorize(Roles="Instructor")]
    public class CourseContentController : ControllerBase
    {
        private readonly ICourseContentService _courseContentService;
        private readonly IContentProgressService _progressService;

        public CourseContentController(ICourseContentService courseContentService,
            IContentProgressService progressService)
        {
            _courseContentService = courseContentService;
            _progressService = progressService;
        }
        [HttpPost("")]
        public async Task<IActionResult> AddContent([FromForm] CourseContentRequest request)
        {
            var instructorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _courseContentService.CreateContent(request, instructorId);
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }
        [HttpGet("{courseId}")]
        public async Task<IActionResult> GetAllCourseContents([FromRoute]int courseId, [FromQuery] string lang = "en")
        {
            var instructorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _courseContentService.GetCourseContents(courseId, instructorId,lang);
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }
        [HttpGet("content/{contentId}")]
        public async Task<IActionResult> GetContent([FromRoute] int contentId, [FromQuery] string lang="en")
        {
            var instructorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _courseContentService.GetContentForInstructor(contentId, instructorId,lang);
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }
        [HttpPut("{contentId}")]
        public async Task<IActionResult> UpdateContent([FromRoute] int contentId, [FromForm] CourseContentRequest request)
        {
            var instructorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _courseContentService.UpdateContent(contentId, request, instructorId);
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }

        [HttpDelete("{contentId}")]
        public async Task<IActionResult> DeleteContent([FromRoute] int contentId)
        {
            var instructorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _courseContentService.DeleteContent(contentId, instructorId);
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }
        [HttpGet("progress/{courseId}")]
        public async Task<IActionResult> GetCourseStudentProgress([FromRoute] int courseId)
        {
            var instructorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _progressService.GetCourseStudentsProgress(courseId,instructorId);   
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }

    }
}
