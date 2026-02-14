using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DAL.DTO.Request.SubmissionRequests
{
    public class SubmissionRequest
    {
        public int TaskItemId { get; set; }
        public IFormFile? Attachment { get; set; }
    }
}
