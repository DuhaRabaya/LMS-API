using LMS.DAL.DTO.Response;
using LMS.DAL.DTO.Response.EnrollmentResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.BLL.Services.EnrollmentsServices
{
    public interface IEnrollmentService
    {
        Task<BaseResponse> Enroll(string studentId, int courseId);
        Task<List<EnrollmentResponse>> GetStudentEnrollments(string studentId, string lang);
    }
}
