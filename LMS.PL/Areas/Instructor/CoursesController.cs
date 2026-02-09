using LMS.BLL.Services.CourseServices;
using LMS.DAL.DTO.Request.CoursesRequests;
using LMS.PL.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LMS.PL.Areas.Instructor
{
    [Route("api/instructor/[controller]")]
    [ApiController]
    [Authorize(Roles = "Instructor")]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }
  
        [HttpGet("all")]
        public async Task<IActionResult> GetCourses()
        {
            var instructorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result =await _courseService.GetCoursesByInstructor(instructorId);
            return Ok(result);
        }

        [HttpPost("")]
        public async Task<IActionResult> createCourse([FromForm] CourseRequest request)
        {
            var instructorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _courseService.CreateCourse(request, instructorId);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }
        [HttpPatch("publish/{id}")]
        public async Task<IActionResult> PublishCourse([FromRoute] int id)
        {
            var instructorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _courseService.SetCoursePublishStatus(id, instructorId, true);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpPatch("unpublish/{id}")]
        public async Task<IActionResult> UnpublishCourse([FromRoute] int id)
        {
            var instructorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _courseService.SetCoursePublishStatus(id, instructorId, false);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateCourse([FromRoute] int id,[FromForm] CourseRequest request)
        {
            var instructorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _courseService.UpdateCourse(id, request, instructorId);
            if (!result.Success)return BadRequest(result);
            return Ok(result);
        }
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteCourse([FromRoute] int id)
        {
            var instructorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _courseService.DeleteCourse(id, instructorId);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }
    }
}
