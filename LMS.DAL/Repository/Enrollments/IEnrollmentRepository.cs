using LMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DAL.Repository.Enrollments
{
    public interface ICheckoutRepository : IRepository<Enrollment>
    {
        Task<List<Enrollment>> GetStudentEnrollments(string studentId, string lang);
        Task<bool> IsEnrolled(string studentId, int courseId);
    }
}
