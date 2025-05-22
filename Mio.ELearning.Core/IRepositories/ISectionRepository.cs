using Mio.ELearning.Core.Models;

namespace Mio.ELearning.Core.Repositories;

public interface ISectionRepository : IGenericRepository<Section>
{
    Task<List<Section>> GetAllWithLessonsAsync(int courseId);
    Task<Section> GetByIdWithLessonsAsync(int sectionId);
}