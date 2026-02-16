using LMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DAL.Repository.ContentProgresses
{
    public interface IContentProgressRepository : IRepository<ContentProgress>
    {
        Task<ContentProgress?> GetProgress(string studentId, int contentId);
        Task<List<ContentProgress>> GetCourseProgress(string studentId, int courseId);
    }
}
