using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DAL.DTO.Response.DashboardResponses
{
    public class StudentCourseDashboardResponse
    {     
          public int CourseId { get; set; }
          public string CourseName { get; set; }
          public decimal ContentProgress { get; set; }
          public decimal TaskProgress { get; set; }
          public decimal OverallProgress { get; set; }
        
    }
}
