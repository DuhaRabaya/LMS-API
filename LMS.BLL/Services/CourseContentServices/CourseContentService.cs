using LMS.BLL.Services.FileServices;
using LMS.DAL.DTO.Request.CourseContentRequests;
using LMS.DAL.DTO.Response;
using LMS.DAL.DTO.Response.CourseContentResponses;
using LMS.DAL.DTO.Response.TaskResponse;
using LMS.DAL.Migrations;
using LMS.DAL.Models;
using LMS.DAL.Repository.CourseContents;
using LMS.DAL.Repository.Courses;
using LMS.DAL.Repository.Enrollments;
using Mapster;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.BLL.Services.CourseContentServices
{
    public class CourseContentService : ICourseContentService
    {
        private readonly ICourseContentRepository _courseContentRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IFileService _fileService;
        private readonly IEnrollmentRepository _enrollmentRepository;

        public CourseContentService(ICourseContentRepository courseContentRepository
            , ICourseRepository courseRepository
            ,IFileService fileService
            ,IEnrollmentRepository enrollmentRepository)
        {
            _courseContentRepository = courseContentRepository;
            _courseRepository = courseRepository;
            _fileService = fileService;
            _enrollmentRepository = enrollmentRepository;
        }
        public async Task<BaseResponse> CreateContent(CourseContentRequest request, string instructorId)
        {
            var course = await _courseRepository.Get(request.CourseId);

            if (course == null || course.InstructorId != instructorId)
                return new BaseResponse
                {
                    Success = false,
                    Message = "Course not found"
                };

            var content = request.Adapt<CourseContent>();

            if (request.AttachmentUrl != null)
            {
                var path = await _fileService.UploadFile(request.AttachmentUrl, "CourseContents");
                content.AttachmentUrl = path;
            }

            var existingOrder = await _courseContentRepository
            .GetCourseContents(request.CourseId);

            if (existingOrder.Any(c => c.Order == request.Order))
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = $"Order {request.Order} already exists in this course."
                };
            }

            await _courseContentRepository.Add(content);
            return new BaseResponse
            {
                Success = true,
                Message = "Content created successfully"
            };
        }

        public async Task<BaseResponse> GetContentForStudent(int contentId, string studentId, string lang = "en")
        {

            var content=await _courseContentRepository.GetContent(contentId);
            if (content == null)
                return new BaseResponse
                {
                    Success = false,
                    Message = "content not found"
                };
            var enrolled = await _enrollmentRepository.IsEnrolled(studentId, content.CourseId);
            if (!enrolled)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "you are not enrolled in this course"
                };
            }
            var result= content.BuildAdapter()
              .AddParameters("lang", lang)
              .AdaptToType<CourseContentResponse>();
            result.Success = true;
            return result;
        }

        public async Task<BaseResponse> GetContentForInstructor(int contentId, string instructorId, string lang = "en")
        {

            var content = await _courseContentRepository.GetContent(contentId);
            if (content == null || content.Course.InstructorId != instructorId)
                return new BaseResponse
                {
                    Success = false,
                    Message = "content not found"
                };
            var result = content.BuildAdapter()
              .AddParameters("lang", lang)
              .AdaptToType<CourseContentResponse>();
            result.Success = true;
            return result;
        }
        public async Task<BaseResponse> GetCourseContents(int courseId, string instructorId, string lang = "en")
        {
             var course= await _courseRepository.Get(courseId);
            if (course == null || course.InstructorId != instructorId)
                return new BaseResponse
                {
                    Success = false,
                    Message = "Course not found"
                };

            var response = (await _courseContentRepository.GetCourseContents(courseId))
              .BuildAdapter()
              .AddParameters("lang", lang)
              .AdaptToType<List<CourseContentResponse>>();
            var contents = new AllCourseContentsResponse()
            {
                Success = true,
                CourseContents = response

            };
            return contents;
        }
       public async Task<BaseResponse> GetCourseContentsForStudent(int courseId, string studentId, string lang = "en")
        {
            var course = await _courseRepository.Get(courseId);
            if (course == null)
                return new BaseResponse
                {
                    Success = false,
                    Message = "Course not found"
                };

            var enrolled =await _enrollmentRepository.IsEnrolled(studentId, courseId);
            if (!enrolled)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "you are not enrolled in this course"
                };
            }
            var response = (await _courseContentRepository.GetActiveByCourse(courseId))
              .BuildAdapter()
              .AddParameters("lang", lang)
              .AdaptToType<List<CourseContentResponse>>();
            var contents = new AllCourseContentsResponse()
            {
                Success = true,
                CourseContents = response

            };
            return contents;
        }

        public async Task<BaseResponse> UpdateContent(int contentId, CourseContentRequest request, string instructorId)
        {
            var content = await _courseContentRepository.GetContent(contentId);
            if (content == null || content.Course.InstructorId != instructorId)
                return new BaseResponse
                {
                    Success = false,
                    Message = "Content not found"
                };
            content.Order = request.Order;
            content.VideoUrl = request.VideoUrl;
            content.IsActive = request.IsActive;

            if (request.AttachmentUrl != null)
            {
                if (!string.IsNullOrEmpty(content.AttachmentUrl))
                {
                    var oldPath = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot",
                        "CourseContents",
                        Path.GetFileName(content.AttachmentUrl));
                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }
                content.AttachmentUrl = await _fileService.UploadFile(request.AttachmentUrl, "CourseContents");
            }
            content.Translations = request.Translations.Adapt<List<CourseContentTranslation>>();

            await _courseContentRepository.Update(content);

            return new BaseResponse
            {
                Success = true,
                Message = "Content updated successfully"
            };
        }

        public async Task<BaseResponse> DeleteContent(int contentId, string instructorId)
        {
            var content = await _courseContentRepository.GetContent(contentId);
            if (content == null || content.Course.InstructorId != instructorId)
                return new BaseResponse
                {
                    Success = false,
                    Message = "Content not found"
                };        
            if (!string.IsNullOrEmpty(content.AttachmentUrl))
            {
                var path = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "CourseContents",
                    Path.GetFileName(content.AttachmentUrl));
                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);
            }

            await _courseContentRepository.Remove(content);

            return new BaseResponse
            {
                Success = true,
                Message = "Content deleted successfully"
            };
        }
        public async Task<BaseResponse> GetNextContent(int contentId, string lang = "en")
        {
            var content = await _courseContentRepository.GetContent(contentId);
            if (content == null)
                return new BaseResponse
                {
                    Success = false,
                    Message = "current content is not found"
                };
            var next = await _courseContentRepository.GetNext(content.CourseId, content.Order);
            if (next == null)
                return new BaseResponse
                {
                    Success = false,
                    Message = "No next content"
                };

            var result = next.BuildAdapter()
                             .AddParameters("lang", lang)
                             .AdaptToType<CourseContentResponse>();

            result.Success = true;
            return result;
        }

        public async Task<BaseResponse> GetPreviousContent(int contentId, string lang = "en")
        {
            var content = await _courseContentRepository.GetContent(contentId);
            if (content == null)
                return new BaseResponse
                {
                    Success = false,
                    Message = "current content is not found"
                };
            var previous = await _courseContentRepository.GetPrevious(content.CourseId,content.Order);
            if (previous == null)
                return new BaseResponse
                {
                    Success = false,
                    Message = "No previous content"
                };

            var result = previous.BuildAdapter()
                                 .AddParameters("lang", lang)
                                 .AdaptToType<CourseContentResponse>();

            result.Success = true;
            return result;
        }



    }
}
