using Azure;
using LMS.BLL.Services.FileServices;
using LMS.DAL.DTO.Request.CoursesRequests;
using LMS.DAL.DTO.Response;
using LMS.DAL.DTO.Response.CoursesResponses;
using LMS.DAL.Migrations;
using LMS.DAL.Models;
using LMS.DAL.Repository;
using LMS.DAL.Repository.Courses;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LMS.BLL.Services.CourseServices
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IFileService _fileService;

        public CourseService(ICourseRepository courseRepository , IFileService fileService)
        {
            _courseRepository = courseRepository;
            _fileService = fileService;
        }
        public async Task<List<CourseResponse>> GetCoursesByInstructor(string id)
        {
            var courses = await _courseRepository.GetAllByInstructor(id);        
            return courses.Adapt<List<CourseResponse>>();
        }
        public async Task<PaginateResponse<CourseResponseForStudent>> GetCoursesForStudent(string lang,int page,int limit
            ,string? search = null, string?instructorId=null, decimal? price=null
            ,decimal? minPrice = null, decimal? maxPrice = null
            , double? minRating = null,double? maxRating = null
            , string? sortBy = null, bool asc = true)
        {
            var query = _courseRepository.Query();
            query = query.Where(c => c.IsPublished);
            if (search is not null)
            {
                query = query.Where(p => p.Translations.Any(t => t.Language == lang &&
                (t.Name.Contains(search) || t.Description.Contains(search))));
            }
            if (instructorId is not null)
            {
                query = query.Where(c=> c.InstructorId==instructorId);
            }
            if (price is not null)
            {
                query = query.Where(c => c.Price == price);
            }
            if (minPrice is not null)
            {
                query = query.Where(c => c.Price >= minPrice);
            }
            if (maxPrice is not null)
            {
                query = query.Where(c => c.Price <= maxPrice);
            }
            if (minRating is not null)
            {
                query = query.Where(c => c.AverageRating >= minRating);
            }
            if (maxRating is not null)
            {
                query = query.Where(c => c.AverageRating <= maxRating);
            }
            if (sortBy is not null)
            {
                sortBy = sortBy.ToLower();

                if (sortBy == "price")
                {
                    query = asc ? query.OrderBy(c => c.Price) : query.OrderByDescending(c => c.Price);
                }
                else if (sortBy == "name")
                {
                    query = asc ? query.OrderBy(c => c.Translations.FirstOrDefault(c => c.Language == lang).Name)
                        : query.OrderByDescending(c => c.Translations.FirstOrDefault(c => c.Language == lang).Name);
                }
                else if (sortBy == "rate")
                {
                    query = asc ? query.OrderBy(c=>c.AverageRating) : query.OrderByDescending(c=>c.AverageRating);
                }
            }
            var total = await query.CountAsync();

            query = query.Skip((page - 1) * limit).Take(limit);
            var response= query
                .BuildAdapter()
                .AddParameters("lang", lang)
                .AdaptToType<List<CourseResponseForStudent>>();

            return new PaginateResponse<CourseResponseForStudent>()
            {
                Total = total,
                Page = page,
                Limit = limit,
                Data = response
            };
        }
        public async Task<List<CourseResponse>> GetCoursesForAdmin()
        {
            return (await _courseRepository.GetAll()).Adapt<List<CourseResponse>>();
        }
        public async Task<BaseResponse> CreateCourse(CourseRequest request , string instructorId)
        {
            var course = request.Adapt<Course>();
            if (request.Thumbnail != null)
            {
                var path = await _fileService.UploadFile(request.Thumbnail);
                course.Thumbnail = path;
            }
            course.InstructorId = instructorId;
            course.IsPublished = false;

            await _courseRepository.Add(course);
            return new BaseResponse { Success = true, Message = "course created successfully" };
        }

        public async Task<BaseResponse> SetCoursePublishStatus(int courseId, string instructorId, bool isPublished)
        {
            var course = await _courseRepository.Get(courseId);
            if (course == null || course.InstructorId != instructorId)
                return new BaseResponse { Success = false, Message = "Course not found or access denied" };

            course.IsPublished = isPublished;
            await _courseRepository.Update(course);
            return new BaseResponse { Success = true, Message = isPublished ? "Course published" : "Course unpublished" };
        }

        public async Task<BaseResponse> UpdateCourse(int courseId, CourseRequest request, string instructorId)
        {
            var course = await _courseRepository.Get(courseId);

            if (course == null || course.InstructorId != instructorId)
                return new BaseResponse
                {
                    Success = false,
                    Message = "Course not found or access denied"
                };
            course.Price = request.Price;
            course.Translations.Clear();
            course.Translations = request.Translations
                .Adapt<List<CourseTranslation>>();
            if (request.Thumbnail != null)
            {
                var path = await _fileService.UploadFile(request.Thumbnail);
                course.Thumbnail = path;
            }

            course.DiscountPercentage = request.DiscountPercentage;
            course.DiscountStartAt = request.DiscountStartAt;
            course.DiscountEndAt = request.DiscountEndAt;

            await _courseRepository.Update(course);

            return new BaseResponse
            {
                Success = true,
                Message = "Course updated successfully"
            };
        }
        public async Task<BaseResponse>  DeleteCourse(int courseId, string instructorId)
        {
            var course = await _courseRepository.Get(courseId);

            if (course == null || course.InstructorId != instructorId)
                return new BaseResponse
                {
                    Success = false,
                    Message = "Course not found or access denied"
                };

            await _courseRepository.Remove(course);

            return new BaseResponse
            {
                Success = true,
                Message = "Course deleted successfully"
            };
        }
    }
}
