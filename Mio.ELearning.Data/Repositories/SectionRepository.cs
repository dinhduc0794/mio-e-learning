using Microsoft.EntityFrameworkCore;
using Mio.ELearning.Core.Models;
using Mio.ELearning.Core.Repositories;

namespace Mio.ELearning.Data.Repositories;

public class SectionRepository : GenericRepository<Section>, ISectionRepository
{
    public SectionRepository(LMSDbContext context) : base(context) { }

    public async Task<List<Section>> GetAllWithCourseAsync()
    {
        return await _context.Sections
            .Include(s => s.Course)
            .Where(s => s.IsActive && !s.IsDeleted)
            .ToListAsync();
    }

    public async Task<Section> GetByIdWithCourseAsync(int id)
    {
        return await _context.Sections
            .Include(s => s.Course)
            .FirstOrDefaultAsync(s => s.SectionId == id && s.IsActive && !s.IsDeleted);
    }

    public async Task<Section> GetByIdWithLessonsAsync(int id)
    {
        return await _context.Sections
            .Include(s => s.Lessons)
            .FirstOrDefaultAsync(s => s.SectionId == id);
    }
}