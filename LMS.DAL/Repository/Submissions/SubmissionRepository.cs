using LMS.DAL.Models;
using LMS.PL.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DAL.Repository.Submissions
{
    public class SubmissionRepository : Repository<Submission>, ISubmissionRepository
    {
        private readonly ApplicationDbContext _context;

        public SubmissionRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Submission?> GetStudentSubmission(string studentId, int taskItemId)
        {
            return await _context.Submissions
                .Include(s => s.Student)      
                .Include(s => s.TaskItem)
                .FirstOrDefaultAsync(s => s.StudentId == studentId && s.TaskItemId == taskItemId);
        }

        public async Task<List<Submission>> GetTaskSubmissions(int taskItemId)
        {
            return await _context.Submissions
                .Include(s => s.Student)
                .Where(s => s.TaskItemId == taskItemId)
                .ToListAsync();
        }
        public async Task<Submission> GetSubmission(int submissionId)
        {
            return await _context.Submissions
                .Include(s => s.TaskItem)
                    .ThenInclude(t => t.Course)
                .FirstOrDefaultAsync(s => s.Id == submissionId);
        }
    }

}
