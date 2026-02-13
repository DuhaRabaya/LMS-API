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
        Task<TaskItem> GetTask(int taskId); 

    }
}
