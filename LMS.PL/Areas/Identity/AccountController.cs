using LMS.BLL.Services.AuthenticationServices;
using LMS.DAL.DTO.Request.LogInRegisterRequests;
using LMS.DAL.DTO.Request.RefreshToken;
using LMS.DAL.DTO.Request.UpdatePasswordRequests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LMS.PL.Areas.Identity
{
    [Route("api/auth/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly IAuthenticationService _authenticationService;

        public AccountController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody]DAL.DTO.Request.LogInRegisterRequests.RegisterRequest request)
        {
            var result = await _authenticationService.RegisterAsync(request);    
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody]DAL.DTO.Request.LogInRegisterRequests.LoginRequest request)
        {
            var result = await _authenticationService.LoginAsync(request);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string token, [FromQuery] string userId)
        {
            var result = await _authenticationService.ConfirmEmailAsync(token, userId);
            if (result)
                return Ok("Email confirmed! You can now log in.");
            
            return BadRequest("Invalid token or user.");
        }

        [HttpPatch("RefreshToken")]
        public async Task<IActionResult> RefreshToken(TokenApiModel request)
        {
            var result = await _authenticationService.RefreshTokenAsync(request);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }
        [HttpPost("SendCode")]
        public async Task<IActionResult> SendCode([FromBody]SendCodeRequest request)
        {
            var result = await _authenticationService.SendCodeAsync(request);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }
        [HttpPut("UpdatePassword")]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordRequest request)
        {
            var result = await _authenticationService.UpdatePasswordAsync(request);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }
        //[Authorize(Roles="Admin")]
        //[HttpPost("upgrade-to-instructor/{id}")]
        //public async Task<IActionResult> UpgradeToInstructor(string id)
        //{
        //    var result=await _authenticationService.UpgradeToInstructor(id);
        //    if (!result.Success) return BadRequest(result);
        //    return Ok(result);
        //}

        //[Authorize(Roles = "Student")]
        //[HttpPost("request-instructor")]
        //public async Task<IActionResult> RequestInstructor()
        //{
        //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    var result = await _authenticationService.RequestInstructorUpgrade(userId);
        //    if (!result.Success) return BadRequest(result);
        //    return Ok(result);
        //}

    }
}

