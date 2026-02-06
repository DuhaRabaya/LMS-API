using LMS.BLL.Services.CourseServices;
using LMS.DAL.DTO.Response.CoursesResponses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LMS.PL.Areas.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class ManagerController : ControllerBase
    {
        
    }
}
