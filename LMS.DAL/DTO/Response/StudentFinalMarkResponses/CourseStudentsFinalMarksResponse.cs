using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DAL.DTO.Response.StudentFinalMarkResponses
{
    public class CourseStudentsFinalMarksResponse : BaseResponse
    {
        public int CourseId { get; set; }
        public List<StudentFinalMarkResponse> Students { get; set; }
    }

}
