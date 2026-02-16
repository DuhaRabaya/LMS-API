using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DAL.Models
{
    public class ContentProgress : BaseModel
    {
        public string StudentId { get; set; }
        public ApplicationUser Student { get; set; }
        public int CourseContentId { get; set; }
        public CourseContent CourseContent { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime? CompletedAt { get; set; }
    }
}
