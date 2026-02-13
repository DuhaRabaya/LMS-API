using Azure;
using LMS.BLL.Services.EnrollmentsServices;
using LMS.BLL.Services.FileServices;
using LMS.DAL.DTO.Request.TaskRequests;
using LMS.DAL.DTO.Response;
using LMS.DAL.DTO.Response.CoursesResponses;
using LMS.DAL.DTO.Response.TaskResponse;
using LMS.DAL.Migrations;
using LMS.DAL.Models;
using LMS.DAL.Repository.Courses;
using LMS.DAL.Repository.Enrollments;
using LMS.DAL.Repository.Tasks;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace LMS.BLL.Services.TaskServices
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IFileService _fileService;
        private readonly IEnrollmentRepository _enrollmentRepository;

        public TaskService(
            ITaskRepository taskRepository,
            ICourseRepository courseRepository,
            IFileService fileService,
            IEnrollmentService enrollmentService,
            IEnrollmentRepository enrollmentRepository)
        {
            _taskRepository = taskRepository;
            _courseRepository = courseRepository;
            _fileService = fileService;
            _enrollmentRepository = enrollmentRepository;
        }

        public async Task<BaseResponse> CreateTask(TaskRequest request, string instructorId)
        {
            var course = await _courseRepository.Get(request.CourseId);
            if (course is null || course.InstructorId != instructorId)
                return new BaseResponse
                {
                    Success = false,
                    Message = "Course not found"
                };
          
            var task=request.Adapt<TaskItem>(); 
   
            if (request.Attachment != null)
                task.AttachmentUrl = await _fileService.UploadFile(request.Attachment, "Tasks");

            await _taskRepository.Add(task);
            return new BaseResponse
            {
                Success = true,
                Message = "Task created successfully"
            };
        }

        public async Task<BaseResponse> UpdateTask(int taskId, TaskRequest request, string instructorId)
        {
            var task = await _taskRepository.GetTask(taskId);
            
            if (task is null || task.Course.InstructorId!=instructorId)
                return new BaseResponse
                {
                    Success = false,
                    Message = "Task not found"
                };

            task.DueDate = request.DueDate;
            task.MaxGrade = request.MaxGrade;
            task.IsActive = request.IsActive;

            if (request.Attachment != null)
            {
                if (!string.IsNullOrEmpty(task.AttachmentUrl))
                {
                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Tasks", task.AttachmentUrl);
                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }
                task.AttachmentUrl = await _fileService.UploadFile(request.Attachment, "Tasks");
            }

            task.Translations = request.Translations.Adapt<List<TaskTranslation>>();
            
            await _taskRepository.Update(task);

            return new BaseResponse
            {
                Success = true,
                Message = "Task updated successfully"
            };
        }

        public async Task<BaseResponse> DeleteTask(int taskId, string instructorId)
        {
            var task = await _taskRepository.GetTask(taskId);
            if (task is null || task.Course.InstructorId!=instructorId)
                return new BaseResponse
                {
                    Success = false,
                    Message = "task not found"
                };

            await _taskRepository.Remove(task);

            return new BaseResponse
            {
                Success = true,
                Message = "Task deleted successfully"
            };
        }

        public async Task<List<TaskResponse>> GetCourseTasks(int courseId , string lang="en")
        {
            var tasks = await _taskRepository.GetTasksByCourse(courseId);
            var response = tasks
               .BuildAdapter()
               .AddParameters("lang", lang)
               .AdaptToType<List<TaskResponse>>();
            
            return response;
        }

        public async Task<BaseResponse> GetActiveCourseTasksForStudent(int courseId , string studentId,string lang="en")
        {
            var isEnrolled = await _enrollmentRepository.IsEnrolled(studentId, courseId);
            if (!isEnrolled) return new BaseResponse()
            {
                 Success = false,
                Message = "student isn't enrolled in this course, can't see it's tasks" 
            };
          
            var tasks = (await _taskRepository.GetActiveTasksByCourse(courseId))
               .BuildAdapter()
               .AddParameters("lang", lang)
               .AdaptToType<List<TaskResponse>>();

            var response = new StudentTasksResponse()
            {
                CourseId = courseId,
                Success = true,
                Tasks = tasks
            };
            return response;
        }

        public async Task<BaseResponse> GetTask(int taskId,string lang="en")
        {
            var task = await _taskRepository.GetTask(taskId);
            if (task == null) return new BaseResponse()
            {
                Success = false,
                Message = "task not found"
            };
            var response = task
               .BuildAdapter()
               .AddParameters("lang", lang)
               .AdaptToType<TaskResponse>();
            response.Success = true;
            return response;
           
        }
        public async Task<List<StudentTasksResponse>> GetAllPendingTasksForStudent(string studentId, string lang = "en")
        {        
            var enrolledCourses = await _enrollmentRepository.GetStudentEnrollments(studentId, lang);
            var allPendingTasks = new List<StudentTasksResponse>();

            foreach (var enrollment in enrolledCourses)
            {
                var courseId = enrollment.Course.Id;
                var tasks= (await _taskRepository.GetActiveTasksByCourse(courseId))
                .BuildAdapter()
                .AddParameters("lang", lang)
                .AdaptToType<List<TaskResponse>>();
                var response = new StudentTasksResponse()
                {
                    CourseId = courseId,
                    Success = true,
                    Tasks = tasks
                };
                allPendingTasks.Add(response);
            }
            return allPendingTasks;
        }

    }
}
