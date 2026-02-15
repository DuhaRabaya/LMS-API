using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DAL.DTO.Response.CourseContentResponses
{
    public class AllCourseContentsResponse : BaseResponse
    {
        public List<CourseContentResponse> CourseContents { get; set; }
    }
}
