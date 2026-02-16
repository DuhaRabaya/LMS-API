using LMS.DAL.Migrations;
using LMS.DAL.Models;
using LMS.PL.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DAL.Repository.Tasks
{
    public class TaskRepository : Repository<TaskItem> , ITaskRepository 
    {
        private readonly ApplicationDbContext _context;

        public TaskRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<TaskItem>> GetTasksByCourse(int courseId)
        {
            return await _context.TaskItems
                 .Include(t=>t.Translations)
                 .Where(t => t.CourseId == courseId)
                 .ToListAsync();
        }

        public async Task<List<TaskItem>> GetActiveTasksByCourse(int courseId)
        {
            return await _context.TaskItems
                 .Where(t => t.CourseId == courseId && t.IsActive)
                 .Include(t => t.Translations)
                 .ToListAsync();
        }
        public async Task<List<TaskItem>> GetPendingTasksByCourse(int courseId, string studentId)
        {
            return await _context.TaskItems
                 .Where(t => t.CourseId == courseId && t.IsActive)
                 .Where(t => !t.Submissions.Any(s => s.StudentId == studentId))
                 .Include(t => t.Translations)
                 .ToListAsync();
        }
        public async Task<TaskItem> GetTask(int taskId)
        {
            return await _context.TaskItems
                .Include(t=>t.Course)
                .Include(t => t.Translations)
                .FirstOrDefaultAsync(t=>t.Id==taskId);
        }
        public async Task<bool> IsTaskCompletedByStudent(int taskId, string studentId)
        {
            return await _context.Submissions
                .AnyAsync(s => s.TaskItemId == taskId && s.StudentId == studentId);
        }

    }
}
