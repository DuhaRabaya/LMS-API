using LMS.DAL.DTO.Response;
using LMS.DAL.DTO.Response.UserResponses;
using LMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.BLL.Services.ManagerServices
{
    public interface IManageUserService
    {
        Task<List<UserResponse>> GetUsers();
        Task<UserDetailsResponse> GetUserDetails(string userId);
        Task<BaseResponse> BlockUser(string userId);
        Task<BaseResponse> UnBLockUser(string userId);
        Task<List<ApplicationUser>> GetPendingInstructors();
        Task<BaseResponse> ApproveInstructor(string userId);
    }
}
