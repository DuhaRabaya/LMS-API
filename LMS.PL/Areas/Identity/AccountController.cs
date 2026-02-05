using LMS.BLL.Services.AuthenticationServices;
using LMS.DAL.DTO.Request.LogInRegisterRequests;
using LMS.DAL.DTO.Request.RefreshToken;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> Register([FromBody]RegisterRequest request)
        {
            var result = await _authenticationService.RegisterAsync(request);    
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody]LoginRequest request)
        {
            var result = await _authenticationService.LoginAsync(request);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        //[HttpGet("ConfirmEmail")]
        //public async Task<IActionResult> ConfirmEmail([FromQuery] string token, [FromQuery] string userId)
        //{
        //    token = Uri.UnescapeDataString(token);
        //    var result = await _authenticationService.ConfirmEmailAsync(token, userId);
        //    if (result)
        //        return Ok("Email confirmed! You can now log in.");
        //    return BadRequest("Invalid token or user.");
        //}

        [HttpPatch("RefreshToken")]
        public async Task<IActionResult> RefreshToken(TokenApiModel request)
        {
            var result = await _authenticationService.RefreshTokenAsync(request);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }
    }
}

