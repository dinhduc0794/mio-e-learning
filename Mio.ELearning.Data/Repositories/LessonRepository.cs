using Microsoft.EntityFrameworkCore;
using Mio.ELearning.Core.Models;
using Mio.ELearning.Core.Repositories;

namespace Mio.ELearning.Data.Repositories;

public class LessonRepository : GenericRepository<Lesson>, ILessonRepository
{
    public LessonRepository(LMSDbContext context) : base(context) { }

    public async Task<List<Lesson>> GetAllWithSectionAsync()
    {
        return await _context.Lessons
            .Include(l => l.Section)
            .Where(l => l.IsActive && !l.IsDeleted)
            .ToListAsync();
    }

    public async Task<Lesson> GetByIdWithSectionAsync(int id)
    {
        return await _context.Lessons
            .Include(l => l.Section)
            .FirstOrDefaultAsync(l => l.LessonId == id && l.IsActive && !l.IsDeleted);
    }
}