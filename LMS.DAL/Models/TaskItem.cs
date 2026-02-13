using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DAL.Models
{
    public class TaskItem : BaseModel
    {
        public DateTime DueDate { get; set; }
        public decimal MaxGrade { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public bool IsActive { get; set; } = true;
        public string? AttachmentUrl { get; set; }
        public List<TaskTranslation> Translations { get; set; }
    }
}
