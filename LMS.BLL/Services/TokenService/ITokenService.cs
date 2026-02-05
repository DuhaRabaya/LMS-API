using LMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LMS.BLL.Services.TokenService
{
    public interface ITokenService
    {
        Task<string> GenerateAccessToken(ApplicationUser user);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
