using LMS.DAL.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.DAL.DTO.Request.LogInRegisterRequests;
using LMS.DAL.DTO.Response.LogInRegisterResponses;
using Mapster;
namespace LMS.BLL.Services.AuthenticationServices
{
    public class AuthenticationService :IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthenticationService(UserManager<ApplicationUser> userManager , SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
            return new LoginResponse()
            {
                Success = true,
                Message = "Login success",
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

            return new RegisterResponse()
            {
                Success = true,
                Message = "account created Successfully"
            };
            
        }
    }
}

