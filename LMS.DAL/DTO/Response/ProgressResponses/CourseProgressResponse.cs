using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DAL.DTO.Response.ProgressResponses
{
    public class CourseProgressResponse : BaseResponse
    {
        public int CourseId { get; set; }
        public int TotalContents { get; set; }
        public int CompletedContents { get; set; }
        public decimal ProgressPercentage { get; set; }   
    }
}
