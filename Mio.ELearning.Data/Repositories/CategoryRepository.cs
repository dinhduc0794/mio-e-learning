using Mio.ELearning.Core.Models;
using Mio.ELearning.Core.Repositories;

namespace Mio.ELearning.Data.Repositories;

public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
{
    public CategoryRepository(LMSDbContext context) : base(context) { }
}