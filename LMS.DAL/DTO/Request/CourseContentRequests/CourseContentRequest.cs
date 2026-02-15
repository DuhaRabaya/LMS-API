using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DAL.DTO.Request.CourseContentRequests
{
    public class CourseContentRequest
    {
        public int CourseId { get; set; }
        public int Order { get; set; }

        public string? VideoUrl { get; set; }
        public IFormFile? AttachmentUrl { get; set; }

        public bool IsActive { get; set; } = true;

        public List<CourseContentTranslationRequest> Translations { get; set; }
    }

}
