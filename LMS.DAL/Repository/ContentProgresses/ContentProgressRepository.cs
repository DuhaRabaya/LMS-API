using LMS.DAL.Models;
using LMS.DAL.Repository.CourseContents;
using LMS.PL.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DAL.Repository.ContentProgresses
{
    public class ContentProgressRepository : Repository<ContentProgress>, IContentProgressRepository
    {
        private readonly ApplicationDbContext _context;

        public ContentProgressRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ContentProgress?> GetProgress(string studentId, int contentId)
        {
            return await _context.ContentProgresses
                .FirstOrDefaultAsync(p =>
                    p.StudentId == studentId &&
                    p.CourseContentId == contentId);
        }
        public async Task<List<ContentProgress>> GetCourseProgress(string studentId, int courseId)
        {
            return await _context.ContentProgresses
                .Include(p => p.CourseContent)
                .Where(p =>
                    p.StudentId == studentId &&
                    p.CourseContent.CourseId == courseId)
                .ToListAsync();
        }
    }
}
