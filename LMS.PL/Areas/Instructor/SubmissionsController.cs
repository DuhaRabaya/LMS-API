using LMS.BLL.Services.SubmissionServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LMS.PL.Areas.Instructor
{
    [Route("api/instructor/[controller]")]
    [ApiController]
    [Authorize(Roles ="Instructor")]
    public class SubmissionsController : ControllerBase
    {
        private readonly ISubmissionService _submissionService;

        public SubmissionsController(ISubmissionService submissionService)
        {
            _submissionService = submissionService;
        }
        [HttpGet("{taskId}")]
        public async Task<IActionResult> GetTaskSubmissions([FromRoute] int taskId) {

            var instructorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _submissionService.GetTaskSubmissions(taskId, instructorId);
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }
    }
}
