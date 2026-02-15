using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DAL.Models
{
    public class CourseContent : BaseModel
    {
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public int Order { get; set; }  
        public string? VideoUrl { get; set; }
        public string? AttachmentUrl { get; set; }
        public bool IsActive { get; set; } = true;
        public List<CourseContentTranslation> Translations { get; set; }
    }
}
