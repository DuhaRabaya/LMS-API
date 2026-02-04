using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DAL.Models
{
    public class CourseTranslation
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public string Language { get; set; } 
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
