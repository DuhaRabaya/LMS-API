using LMS.DAL.DTO.Request.CourseContentRequests;
using LMS.DAL.DTO.Response;
using LMS.DAL.DTO.Response.CourseContentResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.BLL.Services.CourseContentServices
{
    public interface ICourseContentService
    {
        Task<BaseResponse> CreateContent(CourseContentRequest request, string instructorId);
        Task<BaseResponse> UpdateContent(int contentId, CourseContentRequest request, string instructorId);
        Task<BaseResponse> DeleteContent(int contentId, string instructorId);
        Task<BaseResponse> GetCourseContents(int courseId, string instructorId, string lang = "en");
        Task<BaseResponse> GetCourseContentsForStudent(int courseId,string studentsId, string lang = "en");
        Task<BaseResponse> GetContentForStudent(int contentId, string studentId, string lang = "en");
        Task<BaseResponse> GetContentForInstructor(int contentId, string instructorId, string lang = "en");
        Task<BaseResponse> GetNextContent(int contentId, string lang = "en");
        Task<BaseResponse> GetPreviousContent(int contentId, string lang = "en");


    }
}

