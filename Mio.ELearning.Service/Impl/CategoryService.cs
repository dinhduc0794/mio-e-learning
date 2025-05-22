using Mio.ELearning.Core.Models;
using Mio.ELearning.Core.UnitOfWorks;
using Mio.ELearning.Core.ViewModels;

namespace Mio.ELearning.Service.Services.Impl;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ResultViewModel<List<CategoryViewModel>>> GetAllCategoriesAsync()
    {
        var categories = await _unitOfWork.Categories.GetAllAsync();
        var result = categories.Select(c => new CategoryViewModel
        {
            CategoryId = c.CategoryId,
            Name = c.Name,
            Description = c.Description,
            CreatedAt = c.CreatedAt,
            UpdatedAt = c.UpdatedAt,
            IsActive = c.IsActive,
            IsDeleted = c.IsDeleted
        }).ToList();

        return ResultViewModel<List<CategoryViewModel>>.Success(result, "Lấy danh sách danh mục thành công");
    }

    public async Task<ResultViewModel<CategoryViewModel>> GetCategoryByIdAsync(int id)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(id);
        if (category == null)
            return ResultViewModel<CategoryViewModel>.Failure("Không tìm thấy danh mục");

        var viewModel = new CategoryViewModel
        {
            CategoryId = category.CategoryId,
            Name = category.Name,
            Description = category.Description,
            CreatedAt = category.CreatedAt,
            UpdatedAt = category.UpdatedAt,
            IsActive = category.IsActive,
            IsDeleted = category.IsDeleted
        };

        return ResultViewModel<CategoryViewModel>.Success(viewModel, "Lấy chi tiết danh mục thành công");
    }

    public async Task<ResultViewModel<CategoryViewModel>> CreateCategoryAsync(CategoryViewModel viewModel)
    {
        var category = new Category()
        {
            Name = viewModel.Name,
            Description = viewModel.Description,
            CreatedAt = DateTime.UtcNow,
            IsActive = true,
            IsDeleted = false
        };

        await _unitOfWork.Categories.AddAsync(category);
        await _unitOfWork.CompleteAsync();

        viewModel.CategoryId = category.CategoryId;
        viewModel.CreatedAt = category.CreatedAt;

        return ResultViewModel<CategoryViewModel>.Success(viewModel, "Tạo mới danh mục thành công");
    }

    public async Task<ResultViewModel<CategoryViewModel>> UpdateCategoryAsync(CategoryViewModel viewModel)
    {
        var existing = await _unitOfWork.Categories.GetByIdAsync(viewModel.CategoryId);
        if (existing == null)
            return ResultViewModel<CategoryViewModel>.Failure("Không tìm thấy danh mục");

        existing.Name = viewModel.Name;
        existing.Description = viewModel.Description;
        existing.UpdatedAt = DateTime.UtcNow;
        existing.IsActive = viewModel.IsActive;
        existing.IsDeleted = viewModel.IsDeleted;

        await _unitOfWork.CompleteAsync();

        viewModel.UpdatedAt = existing.UpdatedAt;

        return ResultViewModel<CategoryViewModel>.Success(viewModel, "Cập nhật danh mục thành công");
    }

    public async Task<ResultViewModel<bool>> DeleteCategoryAsync(int id)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(id);
        if (category == null)
            return ResultViewModel<bool>.Failure("Không tìm thấy danh mục để xóa");

        var courses = await _unitOfWork.Courses.FindAsync(c => c.CategoryId == id && c.IsActive && !c.IsDeleted);
        if (courses.Any())
            return ResultViewModel<bool>.Failure("Không thể xóa danh mục vì đang có khóa học liên quan");

        _unitOfWork.Categories.Remove(category);
        await _unitOfWork.CompleteAsync();

        return ResultViewModel<bool>.Success(true, "Xóa danh mục thành công");
    }
}
