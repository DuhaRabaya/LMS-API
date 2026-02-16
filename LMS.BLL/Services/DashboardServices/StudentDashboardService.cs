using LMS.BLL.Services.ContentProgressServices;
using LMS.BLL.Services.TaskProgressServices;
using LMS.DAL.DTO.Response.DashboardResponses;
using LMS.DAL.DTO.Response.ProgressResponses;
using LMS.DAL.DTO.Response.TaskProgressResponses;
using LMS.DAL.Repository.Courses;
using LMS.DAL.Repository.Enrollments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.BLL.Services.DashboardServices
{
    public class StudentDashboardService : IStudentDashboardService
    {
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly IContentProgressService _contentProgressService;
        private readonly ITaskProgressService _taskProgressService;
        private readonly ICourseRepository _courseRepository;

        public StudentDashboardService(
            IEnrollmentRepository enrollmentRepository,
            IContentProgressService contentProgressService,
            ITaskProgressService taskProgressService,
            ICourseRepository courseRepository)
        {
            _enrollmentRepository = enrollmentRepository;
            _contentProgressService = contentProgressService;
            _taskProgressService = taskProgressService;
            _courseRepository = courseRepository;
        }

        public async Task<List<StudentCourseDashboardResponse>> GetStudentDashboard(string studentId ,string lang)
        {
            var enrollments = await _enrollmentRepository.GetStudentEnrollments(studentId , lang);
            var dashboard = new List<StudentCourseDashboardResponse>();

            foreach (var enrollment in enrollments)
            {
                var course = enrollment.Course;
                var contentProgressResp = await _contentProgressService.GetCourseProgress(course.Id, studentId)
                                          as CourseProgressResponse;
                var taskProgressResp = await _taskProgressService.GetCourseTaskProgress(course.Id, studentId)
                                       as TaskProgressResponse;

                decimal combinedProgress = 0;
                int count = 0;

                if (contentProgressResp != null)
                {
                    combinedProgress += contentProgressResp.ProgressPercentage;
                    count++;
                }

                if (taskProgressResp != null)
                {
                    combinedProgress += taskProgressResp.ProgressPercentage;
                    count++;
                }

                decimal overallProgress = count > 0 ? Math.Round(combinedProgress / count, 2) : 0;

                dashboard.Add(new StudentCourseDashboardResponse
                {
                    CourseId = course.Id,
                    CourseName = course.Translations.FirstOrDefault(t=> t.Language==lang).Name,
                    ContentProgress = contentProgressResp?.ProgressPercentage ?? 0,
                    TaskProgress = taskProgressResp?.ProgressPercentage ?? 0,
                    OverallProgress = overallProgress
                });
            }

            return dashboard;
        }
    }
}
