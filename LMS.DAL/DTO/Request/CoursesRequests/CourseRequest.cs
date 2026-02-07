using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DAL.DTO.Request.CoursesRequests
{
    public class CourseRequest
    {
        public List<CourseTranslationRequest> Translations { get; set; }
        public double Price { get; set; }
        public bool IsPublished { get; set; } = false;

    }
}
