using LMS.DAL.DTO.Request.CoursesRequests;
using LMS.DAL.DTO.Response;
using LMS.DAL.DTO.Response.CoursesResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.BLL.Services.CourseServices
{
    public interface ICourseService
    {
        Task<BaseResponse> CreateCourse(CourseRequest request, string instructorId);
        Task<List<CourseResponse>> GetCoursesByInstructor(string id);
        Task<List<CourseResponseForAdminStudent>> GetCourses(string lang);

    }
}
