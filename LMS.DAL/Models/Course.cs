using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DAL.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? InstructorId { get; set; }
        public double Price { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsPublished { get; set; } = false;
        public double AverageRating { get; set; } = 0;
        public List<CourseTranslation> Translations { get; set; }   
    }
}
