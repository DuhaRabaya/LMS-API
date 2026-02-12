using LMS.DAL.DTO.Response;
using LMS.DAL.DTO.Response.CoursesResponses;
using LMS.DAL.DTO.Response.EnrollmentResponses;
using LMS.DAL.Migrations;
using LMS.DAL.Models;
using LMS.DAL.Repository.Enrollments;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.BLL.Services.EnrollmentsServices
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IEnrollmentRepository _enrollmentRepository;

        public EnrollmentService(IEnrollmentRepository enrollmentRepository)
        {
            _enrollmentRepository = enrollmentRepository;
        }
        

        public async Task<List<EnrollmentResponse>> GetStudentEnrollments(string studentId, string lang)
        {
            var enrollments = await _enrollmentRepository.GetStudentEnrollments(studentId, lang);
            var result = enrollments.Select(e => new EnrollmentResponse
            {
                EnrolledAt = e.EnrolledAt,
                Course = new CourseResponseForStudent
                {
                    Name = e.Course.Translations.FirstOrDefault()?.Name ?? "No Name",
                    Description = e.Course.Translations.FirstOrDefault()?.Description ?? "No Description",
                    Thumbnail = $"http://localhost:5165/Images/{e.Course.Thumbnail}",
                    FinalPrice = e.Course.FinalPrice,
                    Instructor = e.Course.Instructor.UserName,
                    CreatedAt = e.Course.CreatedAt
                }
            }).ToList();

            return result;
        }

        }
}
