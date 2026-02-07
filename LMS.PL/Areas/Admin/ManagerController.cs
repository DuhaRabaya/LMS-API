using LMS.BLL.Services.AuthenticationServices;
using LMS.BLL.Services.CourseServices;
using LMS.DAL.DTO.Response.CoursesResponses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LMS.PL.Areas.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class ManagerController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public ManagerController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingInstructors()
        {
            var pending = await _authenticationService.GetPendingInstructors();
            return Ok(pending.Select(u => new
            {
                u.Id,
                u.FullName,
                u.Email,
                u.UserName
            }));
        }
        [HttpPatch("approve/{id}")]
        public async Task<IActionResult> ApproveInstructor([FromRoute]string id)
        {
            var result = await _authenticationService.ApproveInstructor(id);
            if (!result.Success) return BadRequest(result);

            return Ok(result);
        }
    }
}
