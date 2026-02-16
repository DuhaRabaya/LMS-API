using LMS.BLL.Services.ContentProgressServices;
using LMS.BLL.Services.TaskProgressServices;
using LMS.DAL.DTO.Response.DashboardResponses;
using LMS.DAL.DTO.Response.ProgressResponses;
using LMS.DAL.DTO.Response.TaskProgressResponses;
using LMS.DAL.Repository.Courses;
using LMS.DAL.Repository.Submissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.BLL.Services.DashboardServices
{
    public class InstructorDashboardService : IInstructorDashboardService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IContentProgressService _contentProgressService;
        private readonly ITaskProgressService _taskProgressService;
        private readonly ISubmissionRepository _submissionRepository;

        public InstructorDashboardService(
            ICourseRepository courseRepository,
            IContentProgressService contentProgressService,
            ITaskProgressService taskProgressService
            ,ISubmissionRepository submissionRepository)
        {
            _courseRepository = courseRepository;
            _contentProgressService = contentProgressService;
            _taskProgressService = taskProgressService;
            _submissionRepository = submissionRepository;
        }

        public async Task<List<InstructorCourseDashboardResponse>> GetInstructorDashboard(string instructorId , string lang)
        {
            var courses = await _courseRepository.GetAllByInstructor(instructorId);
            var dashboard = new List<InstructorCourseDashboardResponse>();
            foreach (var course in courses)
            {
                var contentProgressResp = await _contentProgressService.GetCourseStudentsProgress(course.Id, instructorId)
                                          as CourseStudentsProgressresponse;
                var taskProgressResp = await _taskProgressService.GetCourseStudentsTaskProgress(course.Id, instructorId)
                                       as CourseStudentsTaskProgressResponse;

                var studentsDashboard = new List<StudentProgressForInstructor>();
                decimal passingPercentage = 0;

                if (contentProgressResp != null && taskProgressResp != null)
                {
                    var studentIds = contentProgressResp.Students.Select(s => s.StudentId)
                        .Union(taskProgressResp.Students.Select(s => s.StudentId));

                    int passedCount = 0;
                    int totalStudents = studentIds.Count();
                    foreach (var studentId in studentIds)
                    {
                        var contentProgress = contentProgressResp.Students
                            .FirstOrDefault(s => s.StudentId == studentId)?.ProgressPercentage ?? 0;

                        var taskProgress = taskProgressResp.Students
                            .FirstOrDefault(s => s.StudentId == studentId)?.ProgressPercentage ?? 0;

                        var overall = Math.Round((contentProgress + taskProgress) / 2, 2);
                        
                        var studentName = contentProgressResp.Students
                                          .FirstOrDefault(s => s.StudentId == studentId)?.StudentName
                                          ?? taskProgressResp.Students.FirstOrDefault(s => s.StudentId == studentId)?.StudentName
                                          ?? "Unknown";
                        var grades = await _submissionRepository.GetStudentGradesForCourse(studentId, course.Id);
                        decimal totalGrade = (decimal)grades.Sum();

                        if (totalGrade >= 50)
                            passedCount++;
                        studentsDashboard.Add(new StudentProgressForInstructor
                        {
                            StudentId = studentId,
                            StudentName = studentName,
                            ContentProgress = contentProgress,
                            TaskProgress = taskProgress,
                            OverallProgress = overall
                        });
                    }
                     passingPercentage = totalStudents == 0 ? 0: Math.Round((decimal)passedCount / totalStudents * 100, 2);
                }
              
                dashboard.Add(new InstructorCourseDashboardResponse
                {
                    CourseId = course.Id,
                    CourseName = course.Translations.FirstOrDefault(t => t.Language == lang).Name,
                    Students = studentsDashboard,
                    PassingPercentage = passingPercentage
                });
            }

            return dashboard;
        }
    }
}
