using LMS.DAL.DTO.Response;
using LMS.DAL.DTO.Response.ProgressResponses;
using LMS.DAL.Models;
using LMS.DAL.Repository.ContentProgresses;
using LMS.DAL.Repository.CourseContents;
using LMS.DAL.Repository.Courses;
using LMS.DAL.Repository.Enrollments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.BLL.Services.ContentProgressServices
{
    public class ContentProgressService : IContentProgressService
    {
        private readonly ICourseContentRepository _courseContentRepository;
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly IContentProgressRepository _progressRepository;
        private readonly ICourseRepository _courseRepository;

        public ContentProgressService(ICourseContentRepository courseContentRepository,
            IEnrollmentRepository enrollmentRepository
            ,IContentProgressRepository progressRepository
            ,ICourseRepository courseRepository)
        {
            _courseContentRepository = courseContentRepository;
            _enrollmentRepository = enrollmentRepository;
            _progressRepository = progressRepository;
            _courseRepository = courseRepository;
        }
        public async Task<BaseResponse> CompleteContent(int contentId, string studentId)
        {
            var content = await _courseContentRepository.GetContent(contentId);

            if (content == null)
                return new BaseResponse()
                {
                    Success = false,
                    Message = "content not found"
                };

            var isEnrolled = await _enrollmentRepository
                .IsEnrolled(studentId, content.CourseId);

            if (!isEnrolled)
                return new BaseResponse()
                {
                    Success = false,
                    Message = "not enrolled in this course"
                };

            var progress = await _progressRepository
                .GetProgress(studentId, contentId);

            if (progress != null && progress.IsCompleted)
                return new BaseResponse()
                {
                    Success = false,
                    Message = "already completed"
                };

            if (progress == null)
            {
                progress = new ContentProgress
                {
                    StudentId = studentId,
                    CourseContentId = contentId,
                    IsCompleted = true,
                    CompletedAt = DateTime.UtcNow
                };

                await _progressRepository.Add(progress);
            }
            else
            {
                progress.IsCompleted = true;
                progress.CompletedAt = DateTime.UtcNow;
                await _progressRepository.Update(progress);
            }
            return  new BaseResponse()
            {
                Success = true,
                Message = "marked completed successfully!"
            };
        }

        public async Task<BaseResponse> GetCourseProgress(int courseId, string studentId)
        {
            var contents = await _courseContentRepository
                .GetActiveByCourse(courseId);

            var progressRecords = await _progressRepository
                .GetCourseProgress(studentId, courseId);

            var completedCount = progressRecords.Count(p => p.IsCompleted);
            var totalCount = contents.Count;

            var percentage = totalCount == 0 ? 0 : (decimal)completedCount / totalCount * 100;

            return new CourseProgressResponse
            {
                CourseId = courseId,
                CompletedContents = completedCount,
                TotalContents = totalCount,
                ProgressPercentage = Math.Round(percentage, 2),
                Success = true
            };
        }

        public async Task<BaseResponse> GetCourseStudentsProgress(int courseId, string instructorId)
        {
            var course = await _courseRepository.Get(courseId);

            if (course == null || course.InstructorId != instructorId)
                return new BaseResponse
                {
                    Success = false,
                    Message = "Course not found"
                };

            var contents = await _courseContentRepository.GetActiveByCourse(courseId);
            var totalCount = contents.Count;

            var students = await _courseRepository.GetStudentsInCourse(courseId);

            var result = new List<StudentContentProgressResponse>();

            foreach (var student in students)
            {
                var progressRecords = await _progressRepository
                    .GetCourseProgress(student.Id, courseId);

                var completed = progressRecords.Count(p => p.IsCompleted);

                var percentage = totalCount == 0 ? 0 : (decimal)completed / totalCount * 100;

                result.Add(new StudentContentProgressResponse
                {
                    StudentId = student.Id,
                    StudentName = student.UserName,
                    CompletedContents = completed,
                    TotalContents = totalCount,
                    ProgressPercentage = Math.Round(percentage, 2),
                    Success = true
                });
            }

            return new CourseStudentsProgressresponse
            {
                CourseId = courseId,
                Students = result,
                Success = true
            };
        }

    }
}
