using Mio.ELearning.Core.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Mio.ELearning.Core.Repositories;

public interface ILessonRepository : IGenericRepository<Lesson>
{
    Task<List<Lesson>> GetAllWithSectionAsync();
    Task<Lesson> GetByIdWithSectionAsync(int id);
}