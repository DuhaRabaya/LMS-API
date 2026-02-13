using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DAL.DTO.Response.TaskResponse
{
    public class StudentTasksResponse : BaseResponse
    {
        public int CourseId { get; set; }
        public List<TaskResponse> Tasks { get; set; }

    }
}
