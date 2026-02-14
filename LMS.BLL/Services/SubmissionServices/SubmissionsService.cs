using LMS.BLL.Services.FileServices;
using LMS.DAL.DTO.Request.SubmissionRequests;
using LMS.DAL.DTO.Response;
using LMS.DAL.DTO.Response.SubmissionResponses;
using LMS.DAL.Models;
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

        public SubmissionsService(ITaskRepository taskRepository , 
            IEnrollmentRepository enrollmentRepository,
            ISubmissionRepository submissionRepository,
            IFileService fileService)
        {
            _taskRepository = taskRepository;
            _enrollmentRepository = enrollmentRepository;
            _submissionRepository = submissionRepository;
            _fileService = fileService;
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

        //public async Task<BaseResponse> GradeSubmission(int submissionId, decimal grade, string instructorId)
        //{
        //    var submission = await _submissionRepository.Get(submissionId);

        //    if (submission == null)
        //        return new BaseResponse(false, "Submission not found");

        //    var task = await _taskRepository.GetTask(submission.TaskItemId);

        //    if (task.Course.InstructorId != instructorId)
        //        return new BaseResponse(false, "Unauthorized");

        //    if (grade < 0 || grade > task.MaxGrade)
        //        return new BaseResponse(false, "Invalid grade");

        //    submission.Grade = grade;

        //    await _submissionRepository.Update(submission);

        //    return new BaseResponse(true, "Submission graded");
        //}


    }
}
