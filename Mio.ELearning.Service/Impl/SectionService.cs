using Mio.ELearning.Core.Models;
using Mio.ELearning.Core.UnitOfWorks;
using Mio.ELearning.Core.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace Mio.ELearning.Service.Services.Impl;

public class SectionService : ISectionService
{
    private readonly IUnitOfWork _unitOfWork;

    public SectionService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ResultViewModel<List<SectionViewModel>>> GetAllSectionsAsync(int? courseId = null)
    {
        var sections = await _unitOfWork.Sections.GetAllWithCourseAsync();
        var query = sections.AsQueryable();

        if (courseId.HasValue)
        {
            query = query.Where(s => s.CourseId == courseId.Value);
        }

        var result = query.Select(s => new SectionViewModel
        {
            SectionId = s.SectionId,
            SectionName = s.SectionName,
            Description = s.Description,
            Order = s.Order,
            CourseId = s.CourseId,
            CourseName = s.Course != null ? s.Course.CourseName : "N/A",
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt,
            IsActive = s.IsActive,
            IsDeleted = s.IsDeleted
        }).ToList();

        return ResultViewModel<List<SectionViewModel>>.Success(result, "Lấy danh sách học phần thành công");
    }

    public async Task<ResultViewModel<SectionViewModel>> GetSectionByIdAsync(int id)
    {
        var section = await _unitOfWork.Sections.GetByIdWithCourseAsync(id);
        if (section == null)
            return ResultViewModel<SectionViewModel>.Failure("Không tìm thấy học phần");

        var model = new SectionViewModel
        {
            SectionId = section.SectionId,
            SectionName = section.SectionName,
            Description = section.Description,
            Order = section.Order,
            CourseId = section.CourseId,
            CourseName = section.Course != null ? section.Course.CourseName : "N/A",
            CreatedAt = section.CreatedAt,
            UpdatedAt = section.UpdatedAt,
            IsActive = section.IsActive,
            IsDeleted = section.IsDeleted,
            Lessons = section.Lessons?.Select(l => new LessonViewModel
            {
                LessonId = l.LessonId,
                LessonName = l.LessonName,
                Description = l.Description,
                Content = l.Content,
                VideoUrl = l.VideoUrl,
                DocumentUrl = l.DocumentUrl,
                Order = l.Order
            }).ToList() ?? new List<LessonViewModel>()
        };

        return ResultViewModel<SectionViewModel>.Success(model, "Lấy chi tiết học phần thành công");
    }

    public async Task<ResultViewModel<SectionViewModel>> AddSectionAsync(SectionViewModel model)
    {
        var course = await _unitOfWork.Courses.GetByIdAsync(model.CourseId);
        if (course == null)
            return ResultViewModel<SectionViewModel>.Failure("Khóa học không tồn tại.");

        var section = new Section
        {
            SectionName = model.SectionName,
            Description = model.Description,
            Order = model.Order,
            CourseId = model.CourseId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsActive = true,
            IsDeleted = false
        };

        await _unitOfWork.Sections.AddAsync(section);
        await _unitOfWork.CompleteAsync();

        model.SectionId = section.SectionId;
        model.CreatedAt = section.CreatedAt;

        return ResultViewModel<SectionViewModel>.Success(model, "Tạo học phần thành công");
    }

    public async Task<ResultViewModel<SectionViewModel>> UpdateSectionAsync(SectionViewModel model)
    {
        var existing = await _unitOfWork.Sections.GetByIdAsync(model.SectionId);
        if (existing == null)
            return ResultViewModel<SectionViewModel>.Failure("Không tìm thấy học phần");

        var course = await _unitOfWork.Courses.GetByIdAsync(model.CourseId);
        if (course == null)
            return ResultViewModel<SectionViewModel>.Failure("Khóa học không tồn tại.");

        existing.SectionName = model.SectionName;
        existing.Description = model.Description;
        existing.Order = model.Order;
        existing.CourseId = model.CourseId;
        existing.UpdatedAt = DateTime.UtcNow;
        existing.IsActive = model.IsActive;
        existing.IsDeleted = model.IsDeleted;

        await _unitOfWork.CompleteAsync();

        model.UpdatedAt = existing.UpdatedAt;

        return ResultViewModel<SectionViewModel>.Success(model, "Cập nhật học phần thành công");
    }

    public async Task<ResultViewModel<bool>> DeleteSectionAsync(int id)
    {
        var section = await _unitOfWork.Sections.GetByIdWithLessonsAsync(id);
        if (section == null)
            return ResultViewModel<bool>.Failure("Không tìm thấy học phần");

        if (section.Lessons.Any(l => l.IsActive && !l.IsDeleted))
            return ResultViewModel<bool>.Failure("Không thể xóa học phần vì vẫn còn bài học liên quan");

        _unitOfWork.Sections.Remove(section);
        await _unitOfWork.CompleteAsync();

        return ResultViewModel<bool>.Success(true, "Xóa học phần thành công");
    }
}