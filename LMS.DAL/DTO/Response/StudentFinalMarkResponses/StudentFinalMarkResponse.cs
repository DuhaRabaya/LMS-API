using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DAL.DTO.Response.StudentFinalMarkResponses
{
    public class StudentFinalMarkResponse
    {
        public string StudentId { get; set; }
        public string StudentName { get; set; }
        public decimal FinalMark { get; set; }
    }

}
