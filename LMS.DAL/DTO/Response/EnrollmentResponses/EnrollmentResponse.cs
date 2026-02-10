using LMS.DAL.DTO.Response.CoursesResponses;
using LMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DAL.DTO.Response.EnrollmentResponses
{
    public class EnrollmentResponse
    {
        public DateTime EnrolledAt { get; set; }
        public CourseResponseForStudent Course { get; set; }
    }
}
