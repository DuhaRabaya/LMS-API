using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DAL.DTO.Response.UserResponses
{
    public class UserDetailsResponse :BaseResponse
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool IsBlocked { get; set; }
        public List<string> Roles { get; set; }

    }
}
