using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DAL.DTO.Response.DashboardResponses
{
    public class StudentProgressForInstructor
    {
        public string StudentId { get; set; }
        public string StudentName { get; set; }
        public decimal ContentProgress { get; set; }
        public decimal TaskProgress { get; set; }
        public decimal OverallProgress { get; set; }
    }
}
