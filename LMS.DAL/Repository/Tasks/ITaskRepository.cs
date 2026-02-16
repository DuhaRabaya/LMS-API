using LMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DAL.Repository.Tasks
{
    public interface ITaskRepository : IRepository<TaskItem>
    {
        Task<List<TaskItem>> GetTasksByCourse(int courseId);
        Task<List<TaskItem>> GetActiveTasksByCourse(int courseId);
        Task<List<TaskItem>> GetPendingTasksByCourse(int courseId, string studentId);
        Task<TaskItem> GetTask(int taskId);
        Task<bool> IsTaskCompletedByStudent(int taskId, string studentId);
    }
}
