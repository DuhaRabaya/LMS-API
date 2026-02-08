using LMS.DAL.DTO.Response;
using LMS.DAL.DTO.Response.UserResponses;
using LMS.DAL.Models;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.BLL.Services.ManagerServices
{
    public class ManageUserService : IManageUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ManageUserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<List<UserResponse>> GetUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            var result = users.Adapt<List<UserResponse>>();
            for (int i = 0; i < users.Count; i++)
            {
                var roles = await _userManager.GetRolesAsync(users[i]);
                result[i].Roles = roles.ToList();
            }
            return result;
        }
        public async Task<UserDetailsResponse> GetUserDetails(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return new UserDetailsResponse()
                {
                    Success = false,
                    Message = "user not found"
                };
            }
            var result = user.Adapt<UserDetailsResponse>();
            var roles = await _userManager.GetRolesAsync(user);
            result.Roles = roles.ToList();
            result.Success = true;

            return result;
        }
        public async Task<BaseResponse> BlockUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return new BaseResponse()
                {
                    Success = false,
                    Message = "user not found"
                };
            }
            await _userManager.SetLockoutEnabledAsync(user, true);
            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
            user.IsBlocked = true;
            await _userManager.UpdateAsync(user);
            

            return new BaseResponse()
            {
                Success = true,
                Message = "user blocked"
            };
        }
        public async Task<BaseResponse> UnBLockUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return new BaseResponse()
                {
                    Success = false,
                    Message = "user not found"
                };
            }
            await _userManager.SetLockoutEnabledAsync(user, false);
            await _userManager.SetLockoutEndDateAsync(user, null);
            user.IsBlocked = false;
            await _userManager.UpdateAsync(user);
           
            return new BaseResponse()
            {
                Success = true,
                Message = "user unblocked"
            };
        }   
        public async Task<BaseResponse> ApproveInstructor(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return new BaseResponse { Success = false, Message = "User not found" };

            if (user.IsInstructor)
                return new BaseResponse { Success = false, Message = "User is already an instructor" };

            user.UpgradeToInstructorRequest = false;
            user.IsInstructor = true;


            await _userManager.AddToRoleAsync(user, "Instructor");
            await _userManager.UpdateAsync(user);

            return new BaseResponse { Success = true, Message = "Instructor approved" };
        }
        public async Task<List<ApplicationUser>> GetPendingInstructors()
        {
            var instructors = await _userManager.GetUsersInRoleAsync("Student");
            var pending = instructors.Where(u => u.UpgradeToInstructorRequest).ToList();

            return pending;
        }
    }
}
