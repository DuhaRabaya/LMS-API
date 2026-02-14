using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DAL.DTO.Response.SubmissionResponses
{
    public class TaskSubmissionsResponse : BaseResponse
    {
        public List<SubmissionResponse> Submissions { get; set; }
    }
}
