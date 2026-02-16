using LMS.DAL.DTO.Response.DashboardResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.BLL.Services.DashboardServices
{
    public interface IInstructorDashboardService
    {
        Task<List<InstructorCourseDashboardResponse>> GetInstructorDashboard(string instructorId, string lang);
    }
}
