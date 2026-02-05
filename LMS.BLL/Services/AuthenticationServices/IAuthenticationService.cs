using LMS.DAL.DTO.Request.LogInRegisterRequests;
using LMS.DAL.DTO.Request.RefreshToken;
using LMS.DAL.DTO.Response.LogInRegisterResponses;

namespace LMS.BLL.Services.AuthenticationServices
{
    public interface IAuthenticationService
    {
        Task<RegisterResponse> RegisterAsync(RegisterRequest request);
        Task<LoginResponse> LoginAsync(LoginRequest request);
        Task<bool> ConfirmEmailAsync(string token, string userId);
        Task<LoginResponse> RefreshTokenAsync(TokenApiModel request);
    }
}
