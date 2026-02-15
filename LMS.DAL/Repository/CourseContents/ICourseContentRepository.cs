using LMS.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DAL.Repository.CourseContents
{
    public interface ICourseContentRepository : IRepository<CourseContent>
    {
        Task<List<CourseContent>> GetCourseContents(int courseId);
        Task<List<CourseContent>> GetActiveByCourse(int courseId);
        Task<CourseContent?> GetContent(int contentId);
        Task<CourseContent?> GetNext(int courseId, int order);
        Task<CourseContent?> GetPrevious(int courseId, int order);
    }
}
