using Mio.ELearning.Core.ViewModels;

namespace Mio.ELearning.Service.Services;

public interface ICategoryService
{
    Task<ResultViewModel<List<CategoryViewModel>>> GetAllCategoriesAsync();
    Task<ResultViewModel<CategoryViewModel>> GetCategoryByIdAsync(int id);
    Task<ResultViewModel<CategoryViewModel>> CreateCategoryAsync(CategoryViewModel viewModel);
    Task<ResultViewModel<CategoryViewModel>> UpdateCategoryAsync(CategoryViewModel viewModel);
    Task<ResultViewModel<bool>> DeleteCategoryAsync(int id);
}
