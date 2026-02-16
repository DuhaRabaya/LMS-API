using LMS.DAL.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.BLL.Services.TaskProgressServices
{
    public interface ITaskProgressService
    {
        Task<BaseResponse> GetCourseTaskProgress(int courseId, string studentId);
        Task<BaseResponse> GetCourseStudentsTaskProgress(int courseId, string instructorId);
    }
}
