using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DAL.Models
{
    public class Course :BaseModel
    {
        public string? InstructorId { get; set; }
        public decimal Price { get; set; }
        public bool IsPublished { get; set; } = false;
        public double AverageRating { get; set; } = 0;
        public List<CourseTranslation> Translations { get; set; }   
        public ApplicationUser Instructor { get; set; }
    }
}
