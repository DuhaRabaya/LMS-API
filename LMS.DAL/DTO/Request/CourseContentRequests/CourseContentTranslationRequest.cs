using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DAL.DTO.Request.CourseContentRequests
{
    public class CourseContentTranslationRequest
    {
        public string Language { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }

}
