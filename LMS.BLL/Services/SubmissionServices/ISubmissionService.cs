using LMS.DAL.DTO.Request.SubmissionRequests;
using LMS.DAL.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.BLL.Services.SubmissionServices
{
    public interface ISubmissionService
    {
        Task<BaseResponse> SubmitTask(string studentId, SubmissionRequest request);
        Task<BaseResponse> GetTaskSubmissions(int taskId, string instructorId);
        Task<BaseResponse> GetStudentTaskSubmission(int taskId, string studentId);
        Task<BaseResponse> DeleteSubmission(int submissionId, string studentId);
        Task<BaseResponse> UpdateSubmission(int submissionId, string studentId, SubmissionRequest request);
        Task<BaseResponse> GradeSubmission(int submissionId, GradeSubmissionRequest request,
        string instructorId);

        Task<BaseResponse> RewardTopStudent(int courseId);
    }
}
