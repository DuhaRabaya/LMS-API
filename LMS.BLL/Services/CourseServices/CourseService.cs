using Azure;
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

        public CourseService(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }
        public async Task<List<CourseResponse>> GetCoursesByInstructor(string id)
        {
            var courses = await _courseRepository.GetAllByInstructor(id);        
            return courses.Adapt<List<CourseResponse>>();
        }
        public async Task<PaginateResponse<CourseResponseForStudent>> GetCoursesForStudent(string lang,int page,int limit
            ,string? search = null, string?instructorId=null, decimal? minPrice = null, decimal? maxPrice = null
            , double? minRating = null,double? maxRating = null)
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
            if (minPrice is not null)
            {
                query = query.Where(p => p.Price >= minPrice);
            }
            if (maxPrice is not null)
            {
                query = query.Where(p => p.Price <= maxPrice);
            }
            if (minRating is not null)
            {
                query = query.Where(c => c.AverageRating >= minRating);
            }
            if (maxRating is not null)
            {
                query = query.Where(c => c.AverageRating <= maxRating);
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
            course.InstructorId = instructorId;
            course.IsPublished = false;
            await _courseRepository.Add(course);
            return new BaseResponse { Success = true, Message = "course created successfully" };
        }   
    }
}
