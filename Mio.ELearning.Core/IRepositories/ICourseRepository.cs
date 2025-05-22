using Mio.ELearning.Core.Models;

namespace Mio.ELearning.Core.Repositories;

public interface ICourseRepository : IGenericRepository<Course>
{
    Task<List<Course>> GetAllWithCategoryAsync();
    Task<Course> GetByIdWithSectionsAndLessonsAsync(int id);
}