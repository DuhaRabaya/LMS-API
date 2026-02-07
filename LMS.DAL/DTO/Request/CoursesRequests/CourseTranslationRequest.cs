using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DAL.DTO.Request.CoursesRequests
{
    public class CourseTranslationRequest
    {
        public string Language { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
