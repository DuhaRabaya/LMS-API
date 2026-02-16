using LMS.BLL.Services.TaskProgressServices;
using LMS.BLL.Services.TaskServices;
using LMS.DAL.DTO.Request.TaskRequests;
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
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly ITaskProgressService _taskProgressService;

        public TasksController(ITaskService taskService, ITaskProgressService taskProgressService)
        {
            _taskService = taskService;
            _taskProgressService = taskProgressService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromForm] TaskRequest request)
        {
            var instructorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _taskService.CreateTask(request,instructorId);
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }
        [HttpPut("{taskId}")]
        public async Task<IActionResult> UpdateTask([FromRoute] int taskId, [FromForm] TaskRequest request)
        {
            var instructorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _taskService.UpdateTask(taskId, request , instructorId);
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }

        [HttpDelete("{taskId}")]
        public async Task<IActionResult> DeleteTask([FromRoute]int taskId)
        {
            var instructorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _taskService.DeleteTask(taskId,instructorId);
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }

        [HttpGet("course/{courseId}")]
        public async Task<IActionResult> GetCourseTasks([FromRoute]int courseId, [FromQuery] string lang = "en")
        {
            var response = await _taskService.GetCourseTasks(courseId, lang);
            return Ok(response);
        }

        [HttpGet("students/progress/{courseId}")]
        public async Task<IActionResult> GetCourseStudentsTaskProgress([FromRoute] int courseId)
        {
            var instructorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _taskProgressService.GetCourseStudentsTaskProgress(courseId, instructorId);
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }
    }
}
