using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DAL.Models
{
    public class Enrollment
    {
        public int Id { get; set; }
        public string StudentId { get; set; }
        public ApplicationUser Student { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;
        public string? PaymentId { get; set; }     
    }
}
