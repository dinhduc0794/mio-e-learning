using Mio.ELearning.Core.Models;
using Mio.ELearning.Core.Repositories;

namespace Mio.ELearning.Data.Repositories;

public class LessonRepository : GenericRepository<Lesson>, ILessonRepository
{
    public LessonRepository(LMSDbContext context) : base(context) { }
}