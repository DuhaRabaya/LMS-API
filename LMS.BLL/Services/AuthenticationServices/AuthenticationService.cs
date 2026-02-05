using LMS.BLL.Services.EmailServices;
using LMS.BLL.Services.TokenService;
using LMS.DAL.DTO.Request.LogInRegisterRequests;
using LMS.DAL.DTO.Request.RefreshToken;
using LMS.DAL.DTO.Response.LogInRegisterResponses;
using LMS.DAL.Migrations;
using LMS.DAL.Models;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
namespace LMS.BLL.Services.AuthenticationServices
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;
        private readonly ITokenService _tokenService;

        public AuthenticationService(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            IEmailSender emailSender,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _emailSender = emailSender;
            _tokenService = tokenService;
        }
        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return new LoginResponse()
                {
                    Success = false,
                    Message = "invalid Email, account not found"
                };
            }
            if (await _userManager.IsLockedOutAsync(user))
            {
                return new LoginResponse()
                {
                    Success = false,
                    Message = "Account is Locked! try again later"
                };
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, true);

            if (result.IsLockedOut)
            {
                return new LoginResponse()
                {
                    Success = false,
                    Message = "Account is Locked! too many attempts! try again later"
                };
            }
            else if (result.IsNotAllowed)
            {
                return new LoginResponse()
                {
                    Success = false,
                    Message = "please confirm your email"
                };
            }
            if (!result.Succeeded)
            {
                return new LoginResponse()
                {
                    Success = false,
                    Message = "invalid password"
                };
            }

            var refreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(user);

            return new LoginResponse()
            {
                Success = true,
                Message = "Login success",
                AccessToken = await _tokenService.GenerateAccessToken(user),
                RefreshToken = refreshToken
            };
        }
        public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
        {
            var user = request.Adapt<ApplicationUser>();
            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return new RegisterResponse()
                {
                    Success = false,
                    Message = "User creation failed",
                    Errors = result.Errors.Select(e => e.Description).ToList()

                };
            }

            await _userManager.AddToRoleAsync(user, "Student");

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            token = Uri.EscapeDataString(token);

            var baseUrl = _configuration["ASPNETCORE_ENVIRONMENT"] == "Development"
             ? "http://localhost:5165"
             : "https://learningmanagementsystem.runasp.net";

            var email = $"{baseUrl}/api/auth/Account/ConfirmEmail?token={token}&userId={user.Id}";

            await _emailSender.SendEmail(user.Email, "welcome",
            $"<h1>Welcom .. {user.FullName}</h1> <br> <a href='{email}'>Confirm Email</a>");

            return new RegisterResponse()
            {
                Success = true,
                Message = "account created Successfully"
            };
        }

        public async Task<bool> ConfirmEmailAsync(string token, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) return false;

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded) return false;
            return true;
        }
        public async Task<LoginResponse> RefreshTokenAsync(TokenApiModel request)
        {
            string accessToken = request.AccessToken;
            string refreshToken = request.RefreshToken;

            var principle = _tokenService.GetPrincipalFromExpiredToken(accessToken);
            var userName = principle.Identity.Name;
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == userName);
            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return new LoginResponse()
                {
                    Success = false,
                    Message = "invalid client request"
                };
            }
            var newAccessToken = await _tokenService.GenerateAccessToken(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _userManager.UpdateAsync(user);

            return new LoginResponse()
            {
                Success = true,
                Message = "Token Refreshed Successfully",
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
            };


        }

    }
}



