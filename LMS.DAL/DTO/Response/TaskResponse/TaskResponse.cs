using LMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DAL.DTO.Response.TaskResponse
{
    public class TaskResponse :BaseResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public decimal MaxGrade { get; set; }
        public int CourseId { get; set; }
        public bool IsActive { get; set; } = true;
        public string? AttachmentUrl { get; set; }
    }
}
