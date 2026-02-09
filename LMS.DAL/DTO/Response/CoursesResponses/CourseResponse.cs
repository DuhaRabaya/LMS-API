using LMS.DAL.DTO.Request.CoursesRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DAL.DTO.Response.CoursesResponses
{
    public class CourseResponse : BaseResponse
    {
        public int Id { get; set; }
        public List<CourseTranslationRequest> Translations { get; set; }
        public string? Instructor { get; set; }
        public string InstructorId { get; set; }
        public double Price { get; set; }
        public double AverageRating { get; set; } = 0;
        public bool IsPublished { get; set; }
        public string Thumbnail { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }


    }
}
