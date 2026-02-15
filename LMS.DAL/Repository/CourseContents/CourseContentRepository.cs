using LMS.PL.Data;
using System;
using LMS.DAL.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LMS.DAL.Repository.CourseContents
{
    public class CourseContentRepository : Repository<CourseContent>, ICourseContentRepository
    {
        private readonly ApplicationDbContext _context;

        public CourseContentRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<List<CourseContent>> GetCourseContents(int courseId)
        {
             return await _context.CourseContents.Where(c=>c.CourseId == courseId)
                .Include(c=>c.Translations)
                .OrderBy(c => c.Order)
                .ToListAsync();
        }
        public async Task<List<CourseContent>> GetActiveByCourse(int courseId)
        {
            return await _context.CourseContents
                .Where(c => c.CourseId == courseId && c.IsActive)
                .Include(c => c.Translations)
                .OrderBy(c => c.Order)
                .ToListAsync();
        }
        public async Task<CourseContent?> GetContent(int contentId)
        {
            return await _context.CourseContents
                .Include(c => c.Translations)
                .Include(c => c.Course)
                .FirstOrDefaultAsync(c => c.Id == contentId);
        }
        public async Task<CourseContent?> GetNext(int courseId, int order)
        {
            return await _context.CourseContents
                .Where(c => c.CourseId == courseId && c.Order > order && c.IsActive)
                .OrderBy(c => c.Order)
                .Include(c => c.Translations)
                .FirstOrDefaultAsync();
        }
        public async Task<CourseContent?> GetPrevious(int courseId, int order)
        {
            return await _context.CourseContents
                 .Where(c => c.CourseId == courseId && c.Order < order && c.IsActive)
                 .OrderByDescending(c => c.Order)
                 .Include(c => c.Translations)
                 .FirstOrDefaultAsync();
        }
    }
}
