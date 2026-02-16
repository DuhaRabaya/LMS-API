using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DAL.DTO.Response.TaskProgressResponses
{
    public class CourseStudentsTaskProgressResponse : BaseResponse
    {
        public int CourseId { get; set; }
        public List<StudentTaskProgressResponse> Students { get; set; }
    }
}
