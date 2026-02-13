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

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
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

        [HttpGet("{taskId}")]
        public async Task<IActionResult> GetTask([FromRoute]int taskId, [FromQuery] string lang = "en")
        {
            var response = await _taskService.GetTask(taskId, lang);
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }
        [HttpGet("course/{courseId}")]
        public async Task<IActionResult> GetCourseTasks([FromRoute]int courseId, [FromQuery] string lang = "en")
        {
            var response = await _taskService.GetCourseTasks(courseId, lang);
            return Ok(response);
        }    
    }
}
