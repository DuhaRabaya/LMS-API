using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DAL.DTO.Response.CoursesResponses
{
    public class CourseResponseForStudent
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Thumbnail { get; set; }
        public decimal FinalPrice { get; set; }
        public string Instructor { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
