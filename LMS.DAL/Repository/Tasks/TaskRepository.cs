using LMS.PL.Data;
using LMS.DAL.Models;
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
                 .Where(t => t.CourseId == courseId)
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
    }
}
