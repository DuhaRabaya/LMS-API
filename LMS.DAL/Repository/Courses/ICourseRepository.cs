using LMS.DAL.Models;

namespace LMS.DAL.Repository.Courses
{
    public interface ICourseRepository : IRepository<Course>
    {
        Task<List<Course>> GetAllByInstructor(string instructorId);
    }
}
