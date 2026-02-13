using LMS.DAL.DTO.Request.TaskRequests;
using LMS.DAL.DTO.Response;
using LMS.DAL.DTO.Response.TaskResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.BLL.Services.TaskServices
{
    public interface ITaskService
    {
        Task<BaseResponse> CreateTask(TaskRequest request, string instructorId);
        Task<BaseResponse> UpdateTask(int taskId, TaskRequest request, string instructorId);
        Task<BaseResponse> DeleteTask(int taskId, string instructorId);
        Task<List<TaskResponse>> GetCourseTasks(int courseId ,string lang="en");
        Task<BaseResponse> GetActiveCourseTasksForStudent(int courseId, string studentId, string lang = "en");
        Task<BaseResponse> GetTask(int taskId , string lang="en");
        Task<List<StudentTasksResponse>> GetAllPendingTasksForStudent(string studentId, string lang = "en");
    }
}
