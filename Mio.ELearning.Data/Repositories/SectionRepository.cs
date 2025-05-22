using Microsoft.EntityFrameworkCore;
using Mio.ELearning.Core.Models;
using Mio.ELearning.Core.Repositories;

namespace Mio.ELearning.Data.Repositories;

public class SectionRepository : GenericRepository<Section>, ISectionRepository
{
    public SectionRepository(LMSDbContext context) : base(context) { }
    public async Task<List<Section>> GetAllWithLessonsAsync(int courseId)
    {
        return await _context.Sections
            .Include(s => s.Lessons)
            .Where(s => s.CourseId == courseId && !s.IsDeleted)
            .ToListAsync();
    }

    public async Task<Section> GetByIdWithLessonsAsync(int sectionId)
    {
        return await _context.Sections
            .Include(s => s.Lessons)
            .FirstOrDefaultAsync(s => s.SectionId == sectionId && !s.IsDeleted);
    }
}