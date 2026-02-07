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
        public List<CourseTranslationRequest> Translations { get; set; }
        public string? Instructor { get; set; }
        public double Price { get; set; }
        public double AverageRating { get; set; } = 0;
    }
}
