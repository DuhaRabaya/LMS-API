using LMS.DAL.DTO.Response;
using LMS.DAL.DTO.Response.TaskProgressResponses;
using LMS.DAL.Repository.Courses;
using LMS.DAL.Repository.Enrollments;
using LMS.DAL.Repository.Submissions;
using LMS.DAL.Repository.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.BLL.Services.TaskProgressServices
{
    public class TaskProgressService : ITaskProgressService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ISubmissionRepository _submissionRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IEnrollmentRepository _enrollmentRepository;

        public TaskProgressService(
            ITaskRepository taskRepository,
            ISubmissionRepository submissionRepository,
            ICourseRepository courseRepository,
            IEnrollmentRepository enrollmentRepository)
        {
            _taskRepository = taskRepository;
            _submissionRepository = submissionRepository;
            _courseRepository = courseRepository;
            _enrollmentRepository = enrollmentRepository;
        }
        public async Task<BaseResponse> GetCourseTaskProgress(int courseId, string studentId)
        {
            var enrolled=await _enrollmentRepository.IsEnrolled(studentId, courseId);
            if (!enrolled) {
                return new BaseResponse {
                    Success = false,
                    Message = "not enrolled in this course"
                };
            }
            var tasks = await _taskRepository.GetActiveTasksByCourse(courseId);
            var submissions = await _submissionRepository.GetStudentSubmissionsForCourse(studentId, courseId);

            int totalTasks = tasks.Count;
            int completedTasks = submissions.Count;

            decimal percentage = totalTasks == 0 ? 0 : (decimal)completedTasks / totalTasks * 100;

            return new TaskProgressResponse
            {
                CourseId = courseId,
                TotalTasks = totalTasks,
                CompletedTasks = completedTasks,
                ProgressPercentage = Math.Round(percentage, 2),
                Success = true
            };
        }
        public async Task<BaseResponse> GetCourseStudentsTaskProgress(int courseId, string instructorId)
        {
            var course = await _courseRepository.Get(courseId);
            if (course == null || course.InstructorId != instructorId)
                return new BaseResponse { 
                    Success = false,
                    Message = "Course not found"
                };

            var students = await _courseRepository.GetStudentsInCourse(courseId);
            var tasks = await _taskRepository.GetActiveTasksByCourse(courseId);
            int totalTasks = tasks.Count;

            var result = new List<StudentTaskProgressResponse>();

            foreach (var student in students)
            {
                var submissions = await _submissionRepository.GetStudentSubmissionsForCourse(student.Id, courseId);
                int completedTasks = submissions.Count;

                decimal percentage = totalTasks == 0 ? 0 : (decimal)completedTasks / totalTasks * 100;

                result.Add(new StudentTaskProgressResponse
                {
                    StudentId = student.Id,
                    StudentName = student.UserName,
                    TotalTasks = totalTasks,
                    CompletedTasks = completedTasks,
                    ProgressPercentage = Math.Round(percentage, 2),
                    Success = true
                });
            }

            return new CourseStudentsTaskProgressResponse
            {
                CourseId = courseId,
                Students = result,
                Success = true
            };
        }
    }
}
