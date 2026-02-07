using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DAL.DTO.Response.CoursesResponses
{
    public class CourseTranslationResponse
    {
        public string Language { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
