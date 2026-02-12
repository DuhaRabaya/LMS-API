using LMS.DAL.Models;
using LMS.PL.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DAL.Repository.Enrollments
{
    public class EnrollmentRepository : Repository<Enrollment>, IEnrollmentRepository
    {
        private readonly ApplicationDbContext _context;

        public EnrollmentRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<List<Enrollment>> GetStudentEnrollments(string studentId, string lang)
        {
            return await _context.Enrollments
            .Where(e => e.StudentId == studentId)
            .Include(e => e.Course)
            .ThenInclude(c => c.Translations.Where(t => t.Language == lang))
            .Include(e => e.Course)
            .ThenInclude(c => c.Instructor)
            .ToListAsync();
        }
        public async Task<Enrollment> GetEnrollment(string studentId, int courseId)
        {
            return await _context.Enrollments.FirstOrDefaultAsync(e=>e.StudentId==studentId && e.CourseId==courseId);
        }
        public async Task<bool> IsEnrolled(string studentId, int courseId)
        {
            return await _context.Enrollments.AnyAsync(e => e.StudentId == studentId && e.CourseId == courseId);
        }
    }
}
