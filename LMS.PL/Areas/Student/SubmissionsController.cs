using LMS.BLL.Services.SubmissionServices;
using LMS.DAL.DTO.Request.SubmissionRequests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LMS.PL.Areas.Student
{
    [Route("api/student/[controller]")]
    [ApiController]
    [Authorize(Roles ="Student")]
    public class SubmissionsController : ControllerBase
    {
        private readonly ISubmissionService _submissionService;

        public SubmissionsController(ISubmissionService submissionService)
        {
            _submissionService = submissionService;
        }
        
        [HttpPost("")]
        public async Task<IActionResult> SubmitTask([FromForm] SubmissionRequest request)
        {
            var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);      
            var response = await _submissionService.SubmitTask(studentId, request);
            if (!response.Success)   return BadRequest(response);
            return Ok(response);
        }
        [HttpGet("{taskId}")]
        public async Task<IActionResult> GetTaskSubmission([FromRoute] int taskId)
        {

            var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _submissionService.GetStudentTaskSubmission(taskId, studentId);
            if (!response.Success) return BadRequest(response);
            return Ok(response);

        }
        [HttpDelete("delete/{submissionId}")]
        public async Task<IActionResult> DeleteSubmission([FromRoute] int submissionId)
        {
            var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _submissionService.DeleteSubmission(submissionId, studentId);
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }
        [HttpPut("update/{submissionId}")]
        public async Task<IActionResult> UpdateSubmission([FromRoute] int submissionId , [FromForm] SubmissionRequest request)
        {
            var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _submissionService.UpdateSubmission(submissionId, studentId,request);
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }

    }
}
