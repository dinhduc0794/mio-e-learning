using Mio.ELearning.Core.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Mio.ELearning.Core.Repositories;

public interface ISectionRepository : IGenericRepository<Section>
{
    Task<List<Section>> GetAllWithCourseAsync();
    Task<Section> GetByIdWithCourseAsync(int id);
    Task<Section> GetByIdWithLessonsAsync(int id);
}