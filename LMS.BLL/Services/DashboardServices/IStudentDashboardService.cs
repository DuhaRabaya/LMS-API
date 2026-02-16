using LMS.DAL.DTO.Response.DashboardResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.BLL.Services.DashboardServices
{
    public interface IStudentDashboardService
    {
        Task<List<StudentCourseDashboardResponse>> GetStudentDashboard(string studentId, string lang);
    }
}
