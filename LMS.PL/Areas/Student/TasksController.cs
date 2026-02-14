using Azure;
using LMS.BLL.Services.TaskServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LMS.PL.Areas.Student
{
    [Route("api/student/[controller]")]
    [ApiController]
    [Authorize(Roles ="Student")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet("course/{courseId}")]
        public async Task<IActionResult> GetActiveCourseTasks(int courseId, [FromQuery] string lang = "en")
        {
            var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _taskService.GetActiveCourseTasksForStudent(courseId,studentId,lang);
            if (!response.Success) return BadRequest(response);
            return Ok(response);

        }
        [HttpGet("{taskId}")]
        public async Task<IActionResult> GetTask(int taskId, [FromQuery] string lang = "en")
        {
            var response = await _taskService.GetTask(taskId, lang);
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }
        [HttpGet("alltasks")]
        public async Task<IActionResult> GetAllPendingTask([FromQuery] string lang = "en")
        {
            var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _taskService.GetAllPendingTasksForStudent(studentId,lang);
            return Ok(response);
        }

    }
}
