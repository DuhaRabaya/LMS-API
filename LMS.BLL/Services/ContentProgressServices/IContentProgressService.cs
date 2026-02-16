using LMS.DAL.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.BLL.Services.ContentProgressServices
{
    public interface IContentProgressService
    {
        Task<BaseResponse> CompleteContent(int contentId, string studentId);
        Task<BaseResponse> GetCourseProgress(int courseId, string studentId);
        Task<BaseResponse> GetCourseStudentsProgress(int courseId, string instructorId);
    }
}
