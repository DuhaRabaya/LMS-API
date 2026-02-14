using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DAL.Models
{
    public class Submission : BaseModel
    {
        public int Id { get; set; }
        public string StudentId { get; set; }
        public ApplicationUser Student { get; set; }
        public int TaskItemId { get; set; }
        public TaskItem TaskItem { get; set; }
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
        public string? AttachmentUrl { get; set; }
        public bool IsLate { get; set; }
        public decimal? Grade { get; set; }
        public string? Feedback { get; set; }
        public DateTime? GradedAt { get; set; }
    }
}
