using LMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DAL.DTO.Response.ProgressResponses
{
    public class StudentContentProgressResponse : BaseResponse
    {
        public string StudentId { get; set; }
        public string StudentName { get; set; }
        public int CompletedContents { get; set; }
        public int TotalContents { get; set; }
        public decimal ProgressPercentage { get; set; }
    }
}
