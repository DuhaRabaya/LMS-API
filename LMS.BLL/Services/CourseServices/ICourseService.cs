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
        Task<List<CourseResponse>> GetCoursesForAdmin();
        Task<PaginateResponse<CourseResponseForStudent>> GetCoursesForStudent(string lang, int page, int limit
             , string? search = null, string? instructorId = null, decimal? price = null
             , decimal? minPrice = null, decimal? maxPrice = null
             , double? minRating = null, double? maxRating = null
             , string? sortBy = null, bool asc = true);

        Task<BaseResponse> SetCoursePublishStatus(int courseId, string instructorId, bool isPublished);


    }
}
