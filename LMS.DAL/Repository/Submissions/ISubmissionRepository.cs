using LMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DAL.Repository.Submissions
{
    public interface ISubmissionRepository : IRepository<Submission>
    {
        Task<Submission?> GetStudentSubmission(string studentId, int taskItemId);
        Task<List<Submission>> GetTaskSubmissions(int taskItemId);
    }
}
