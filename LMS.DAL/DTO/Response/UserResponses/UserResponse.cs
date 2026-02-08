using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DAL.DTO.Response.UserResponses
{
    public class UserResponse : BaseResponse
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool IsBlocked { get; set; }
        public List<string> Roles { get; set; }
    }
}
