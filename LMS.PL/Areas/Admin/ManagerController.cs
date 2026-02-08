using LMS.BLL.Services.AuthenticationServices;
using LMS.BLL.Services.CourseServices;
using LMS.BLL.Services.ManagerServices;
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
       
        private readonly IManageUserService _manageUserService;

        public ManagerController(IManageUserService manageUserService)
        {
            _manageUserService = manageUserService;
        }
        [HttpGet("")]
        public async Task<IActionResult> GetUsers() => Ok(await _manageUserService.GetUsers());
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserDetails([FromRoute] string id)
        {
            var response = await _manageUserService.GetUserDetails(id);
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }

        [HttpPatch("block/{id}")]
        public async Task<IActionResult> BlockUser([FromRoute] string id)
        {
            var response = await _manageUserService.BlockUser(id);
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }

        [HttpPatch("unblock/{id}")]
        public async Task<IActionResult> UnBlockUser([FromRoute] string id)
        {
            var response = await _manageUserService.UnBLockUser(id);
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }
        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingInstructors()
        {
            var pending = await _manageUserService.GetPendingInstructors();
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
            var result = await _manageUserService.ApproveInstructor(id);
            if (!result.Success) return BadRequest(result);

            return Ok(result);
        }

    }
}
