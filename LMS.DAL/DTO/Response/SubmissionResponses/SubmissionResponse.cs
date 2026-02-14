using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DAL.DTO.Response.SubmissionResponses
{
    public class SubmissionResponse : BaseResponse
    {
        public int Id { get; set; }
        public string StudentName { get; set; }    
        public int TaskItemId { get; set; }
        public DateTime SubmittedAt { get; set; }
        public string? AttachmentUrl { get; set; }
        public decimal? Grade { get; set; }
        public string? Feedback { get; set; }
        public bool IsLate { get; set; }
    }
}
