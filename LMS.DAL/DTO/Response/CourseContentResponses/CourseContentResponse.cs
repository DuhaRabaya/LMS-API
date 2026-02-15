using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DAL.DTO.Response.CourseContentResponses
{
    public class CourseContentResponse : BaseResponse
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int Order { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? VideoUrl { get; set; }
        public string? AttachmentUrl { get; set; }
        public bool IsActive { get; set; }
    }
}
