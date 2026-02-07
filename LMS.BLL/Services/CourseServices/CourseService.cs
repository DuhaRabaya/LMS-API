using LMS.DAL.DTO.Request.CoursesRequests;
using LMS.DAL.DTO.Response;
using LMS.DAL.DTO.Response.CoursesResponses;
using LMS.DAL.Migrations;
using LMS.DAL.Models;
using LMS.DAL.Repository;
using LMS.DAL.Repository.Courses;
using Mapster;
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
        public async Task<List<CourseResponseForAdminStudent>> GetCourses(string lang)
        {
            var courses = await _courseRepository.GetAll();

            return courses
                .BuildAdapter()
                .AddParameters("lang", lang)
                .AdaptToType<List<CourseResponseForAdminStudent>>();
        }
        public async Task<BaseResponse> CreateCourse(CourseRequest request , string instructorId)
        {
            var course = request.Adapt<Course>();
            course.InstructorId = instructorId;
            await _courseRepository.Add(course);
            return new BaseResponse { Success = true, Message = "course created successfully" };
        }

       
    }
}
