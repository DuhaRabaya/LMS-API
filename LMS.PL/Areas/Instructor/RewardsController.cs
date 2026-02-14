using LMS.BLL.Services.CourseServices;
using LMS.BLL.Services.RefundServices;
using LMS.BLL.Services.SubmissionServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LMS.PL.Areas.Instructor
{
    [Route("api/instructor/[controller]")]
    [ApiController]
    [Authorize(Roles ="Instructor")]
    public class RewardsController : ControllerBase
    {
        private readonly ISubmissionService _submissionService;

        public RewardsController(ISubmissionService submissionService)
        {
            _submissionService = submissionService;
        }

        [HttpPost("reward/{courseId}")]
        public async Task<IActionResult> RewardStudent(int courseId)
        {
            var response = await _submissionService.RewardTopStudent(courseId);
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }
    }
    }
