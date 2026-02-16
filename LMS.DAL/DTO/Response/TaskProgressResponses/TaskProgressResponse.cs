using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DAL.DTO.Response.TaskProgressResponses
{ 
    public class TaskProgressResponse : BaseResponse
    {
        public int CourseId { get; set; }
        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public decimal ProgressPercentage { get; set; }
    }
}
