using LMS.DAL.Models;
using LMS.PL.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DAL.Repository.Courses
{
    public class CourseRepository : Repository<Course> , ICourseRepository
    {
        private readonly ApplicationDbContext _context;

        public CourseRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Course>> GetAll()
        {
            return await _context.Courses
                .Include(c => c.Instructor).Include(c=>c.Translations)
                .ToListAsync();
        }
        public async Task<List<Course>> GetAllByInstructor(string instructorId)
        {
            return await _context.Courses.Include(c => c.Instructor).Include(c=>c.Translations)
                .Where(c=>c.InstructorId==instructorId).ToListAsync();
        }
        public IQueryable<Course> Query()
        {
            return _context.Courses.Include(c => c.Translations).Include(c=>c.Instructor).AsQueryable();
        }
    }
}
  