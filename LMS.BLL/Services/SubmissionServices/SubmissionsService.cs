using LMS.BLL.Services.EmailServices;
using LMS.BLL.Services.FileServices;
using LMS.BLL.Services.RefundServices;
using LMS.DAL.DTO.Request.SubmissionRequests;
using LMS.DAL.DTO.Response;
using LMS.DAL.DTO.Response.SubmissionResponses;
using LMS.DAL.Migrations;
using LMS.DAL.Models;
using LMS.DAL.Repository.Courses;
using LMS.DAL.Repository.Enrollments;
using LMS.DAL.Repository.Submissions;
using LMS.DAL.Repository.Tasks;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.BLL.Services.SubmissionServices
{
    public class SubmissionsService : ISubmissionService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly ISubmissionRepository _submissionRepository;
        private readonly IFileService _fileService;
        private readonly IStripeRefundService _stripeRefundService;
        private readonly ICourseRepository _courseRepository;
        private readonly IEmailSender _emailSender;

        public SubmissionsService(ITaskRepository taskRepository , 
            IEnrollmentRepository enrollmentRepository,
            ISubmissionRepository submissionRepository,
            IFileService fileService,
            IStripeRefundService stripeRefundService,
            ICourseRepository courseRepository,
            IEmailSender emailSender)
        {
            _taskRepository = taskRepository;
            _enrollmentRepository = enrollmentRepository;
            _submissionRepository = submissionRepository;
            _fileService = fileService;
            _stripeRefundService = stripeRefundService;
            _courseRepository = courseRepository;
            _emailSender = emailSender;
        }
        public async Task<BaseResponse> SubmitTask(string studentId, SubmissionRequest request)
        {
            var task = await _taskRepository.GetTask(request.TaskItemId);

            if (task == null)
                return new BaseResponse()
                {
                    Success = false,
                    Message = "task not found"
                };

            var isEnrolled = await _enrollmentRepository.IsEnrolled(studentId, task.CourseId);
            if (!isEnrolled)
                return new BaseResponse()
                {
                    Success = false,
                    Message = "student is not enrolled in this course"
                };

            var existingSubmission = await _submissionRepository
                .GetStudentSubmission(studentId, request.TaskItemId);

            if (existingSubmission != null)
                return new BaseResponse()
                {
                    Success = false,
                    Message = "solution is already submitted"
                };
            if (DateTime.UtcNow > task.DueDate || !task.IsActive)
            {
                return new BaseResponse()
                {
                    Success = false,
                    Message = "task deadline has passed, you're late!"
                };
            }
             var submission = new Submission
            {
                StudentId = studentId,
                TaskItemId = request.TaskItemId,
                IsLate = DateTime.UtcNow > task.DueDate
            };

            if (request.Attachment != null)
                submission.AttachmentUrl = await _fileService.UploadFile(request.Attachment, "Submissions");

            await _submissionRepository.Add(submission);

            return new BaseResponse()
            {
                Success = true,
                Message = "submission successfull"
            };
        }
        public async Task<BaseResponse> DeleteSubmission(int submissionId ,string studentId)
        {
            var submission= await _submissionRepository.Get(submissionId);
            if (submission.StudentId != studentId)
                return new BaseResponse()
                {
                    Success = false,
                    Message = "not allowed to delete this submission"
                };
            if (submission is null)
            {
                return new BaseResponse()
                {
                    Success = false,
                    Message = "submission not found"
                };
            }
            await _submissionRepository.Remove(submission);
            return new BaseResponse()
            {
                Success = true,
                Message = "submission deleted successfully"
            };
        }
        public async Task<BaseResponse> UpdateSubmission(int submissionId, string studentId , SubmissionRequest request)
        {
            var task=await _taskRepository.Get(request.TaskItemId);
            if (task == null)
                return new BaseResponse()
                {
                    Success = false,
                    Message = "task not found"
                };

            if (DateTime.UtcNow > task.DueDate || !task.IsActive)
            {
                return new BaseResponse()
                {
                    Success = false,
                    Message = "task deadline has passed, you're late!"
                };
            }
            if (task == null)
                return new BaseResponse()
                {
                    Success = false,
                    Message = "task not found"
                };

            var isEnrolled = await _enrollmentRepository.IsEnrolled(studentId, task.CourseId);
            if (!isEnrolled)
                return new BaseResponse()
                {
                    Success = false,
                    Message = "student is not enrolled in this course"
                };
            var submission=await _submissionRepository.Get(submissionId);
            if (submission is null)
            {
                return new BaseResponse()
                {
                    Success = false,
                    Message = "submission not found"
                };
            }

            if (request.Attachment != null)
            {
                if (!string.IsNullOrEmpty(submission.AttachmentUrl))
                {
                    var oldPath = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot",
                        "Submissions",
                        submission.AttachmentUrl);

                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }

                submission.AttachmentUrl = await _fileService.UploadFile(request.Attachment, "Submissions");
            }
            await _submissionRepository.Update(submission);
            return new BaseResponse()
            {
                Success = true,
                Message = "submission updated successfully"
            };
        }
        public async Task<BaseResponse> GetTaskSubmissions(int taskId , string instructorId)
        {
            var task =await _taskRepository.GetTask(taskId);
            if (task == null)
                return new BaseResponse()
                {
                    Success = false,
                    Message = "task not found"
                };

            if (task.Course.InstructorId != instructorId)
            {
                return new BaseResponse()
                {
                    Success = false,
                    Message = "instructor not allowed"
                };
            }
            var response=await _submissionRepository.GetTaskSubmissions(taskId);
            var result = new TaskSubmissionsResponse()
            {
                Submissions = response.Adapt<List<SubmissionResponse>>()
            };
            result.Success = true;
            return result;
        }
        public async Task<BaseResponse> GetStudentTaskSubmission(int taskId , string studentId)
        {
            var submission=await _submissionRepository.GetStudentSubmission(studentId, taskId);
            if (submission is null)
            {
                return new BaseResponse()
                {
                    Success = false,
                    Message = "submission not found"
                };
            }
            var response= submission.Adapt<SubmissionResponse>();
            response.Success = true;
            return response;
        }

        public async Task<BaseResponse> GradeSubmission(int submissionId, GradeSubmissionRequest request,
        string instructorId)
        {
            var submission = await _submissionRepository.GetSubmission(submissionId);

            if (submission == null)
                return new BaseResponse
                {
                    Success = false,
                    Message = "Submission not found"
                };

            if (submission.TaskItem.Course.InstructorId != instructorId)
                return new BaseResponse
                {
                    Success = false,
                    Message = "instructor not allowed"
                };

            if (request.Grade < 0 || request.Grade > submission.TaskItem.MaxGrade)
                return new BaseResponse
                {
                    Success = false,
                    Message = $"Grade must be between 0 and {submission.TaskItem.MaxGrade}"
                };

            submission.Grade = request.Grade;
            submission.Feedback = request.Feedback;
            submission.GradedAt = DateTime.UtcNow;

            await _submissionRepository.Update(submission);

            return new BaseResponse
            {
                Success = true,
                Message = "Submission graded successfully"
            };
        }

        public async Task<BaseResponse> RewardTopStudent(int courseId)
        {
            var course = await _courseRepository.Get(courseId);
            if (course == null)
                return new BaseResponse { Success = false, Message = "Course not found" };

            var tasks = await _taskRepository.GetTasksByCourse(courseId);
            if (!tasks.Any())
                return new BaseResponse { Success = false, Message = "No tasks in this course" };

            var students = await _courseRepository.GetStudentsInCourse(courseId);
            if (!students.Any())
                return new BaseResponse { Success = false, Message = "No students enrolled" };

            string topStudentId = null;
            ApplicationUser topStudent= null;
            decimal maxTotalGrade = -1;
            foreach (var student in students)
            {
                var submissions = await _submissionRepository.GetStudentSubmissionsForCourse(student.Id, courseId);         
                if (submissions.Count != tasks.Count) continue;
                var totalGrade = submissions.Sum(s => s.Grade ?? 0);
                if (totalGrade > maxTotalGrade)
                {
                    maxTotalGrade = totalGrade;
                    topStudentId = student.Id;
                    topStudent= student;
                    
                }
            }
            if (topStudentId == null)
                return new BaseResponse { Success = false, Message = "No eligible top student" };

            var refundResult = await _stripeRefundService.RewardTopStudentAsync(topStudentId, courseId);
            if (!refundResult.Success)
                return refundResult;

            await _emailSender.SendEmail(
              "duharabaya4@gmail.com",
               "Congratulations! You got a reward",
               $"You have received a refund for excelling in the course {course.Translations .FirstOrDefault(c => c.Language == "en").Name}."
           );
            return new BaseResponse
            {
                Success = true,
                Message = $"Top student rewarded! Refund processed for student {topStudentId}.",
            };
        }

    }
}
