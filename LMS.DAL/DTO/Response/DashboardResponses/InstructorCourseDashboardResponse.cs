using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DAL.DTO.Response.DashboardResponses
{
    public class InstructorCourseDashboardResponse
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public List<StudentProgressForInstructor> Students { get; set; }
    }
}
