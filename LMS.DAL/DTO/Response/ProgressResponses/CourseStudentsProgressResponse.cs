using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DAL.DTO.Response.ProgressResponses
{
    public class CourseStudentsProgressresponse : BaseResponse
    {
        public int CourseId { get; set; }
        public List<StudentContentProgressResponse> Students { get; set; }
    }
}
