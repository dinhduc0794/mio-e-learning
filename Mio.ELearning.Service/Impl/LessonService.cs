using Mio.ELearning.Core.Models;
using Mio.ELearning.Core.UnitOfWorks;
using Mio.ELearning.Core.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace Mio.ELearning.Service.Services.Impl;

public class LessonService : ILessonService
{
    private readonly IUnitOfWork _unitOfWork;

    public LessonService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ResultViewModel<List<LessonViewModel>>> GetAllLessonsAsync(int? sectionId = null)
    {
        var lessons = await _unitOfWork.Lessons.GetAllWithSectionAsync();
        var query = lessons.AsQueryable();

        if (sectionId.HasValue)
        {
            query = query.Where(l => l.SectionId == sectionId.Value);
        }

        var result = query
            .OrderBy(l => l.Order) 
            .Select(l => new LessonViewModel
        {
            LessonId = l.LessonId,
            LessonName = l.LessonName,
            Description = l.Description,
            Content = l.Content,
            VideoUrl = l.VideoUrl,
            DocumentUrl = l.DocumentUrl,
            Order = l.Order,
            SectionId = l.SectionId,
            SectionName = l.Section != null ? l.Section.SectionName : "N/A",
            CreatedAt = l.CreatedAt,
            UpdatedAt = l.UpdatedAt,
            IsActive = l.IsActive,
            IsDeleted = l.IsDeleted
        }).ToList();

        return ResultViewModel<List<LessonViewModel>>.Success(result, "Lấy danh sách bài học thành công");
    }

    public async Task<ResultViewModel<LessonViewModel>> GetLessonByIdAsync(int id)
    {
        var lesson = await _unitOfWork.Lessons.GetByIdWithSectionAsync(id);
        if (lesson == null)
            return ResultViewModel<LessonViewModel>.Failure("Không tìm thấy bài học");

        var model = new LessonViewModel
        {
            LessonId = lesson.LessonId,
            LessonName = lesson.LessonName,
            Description = lesson.Description,
            Content = lesson.Content,
            VideoUrl = lesson.VideoUrl,
            DocumentUrl = lesson.DocumentUrl,
            Order = lesson.Order,
            SectionId = lesson.SectionId,
            SectionName = lesson.Section != null ? lesson.Section.SectionName : "N/A",
            CreatedAt = lesson.CreatedAt,
            UpdatedAt = lesson.UpdatedAt,
            IsActive = lesson.IsActive,
            IsDeleted = lesson.IsDeleted
        };

        return ResultViewModel<LessonViewModel>.Success(model, "Lấy chi tiết bài học thành công");
    }

    public async Task<ResultViewModel<LessonViewModel>> AddLessonAsync(LessonViewModel model)
    {
        var section = await _unitOfWork.Sections.GetByIdAsync(model.SectionId);
        if (section == null)
            return ResultViewModel<LessonViewModel>.Failure("Học phần không tồn tại.");

        var lesson = new Lesson
        {
            LessonName = model.LessonName,
            Description = model.Description,
            Content = model.Content,
            VideoUrl = model.VideoUrl,
            DocumentUrl = model.DocumentUrl,
            Order = model.Order,
            SectionId = model.SectionId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsActive = true,
            IsDeleted = false
        };

        await _unitOfWork.Lessons.AddAsync(lesson);
        await _unitOfWork.CompleteAsync();

        model.LessonId = lesson.LessonId;
        model.CreatedAt = lesson.CreatedAt;

        return ResultViewModel<LessonViewModel>.Success(model, "Tạo bài học thành công");
    }

    public async Task<ResultViewModel<LessonViewModel>> UpdateLessonAsync(LessonViewModel model)
    {
        var existing = await _unitOfWork.Lessons.GetByIdAsync(model.LessonId);
        if (existing == null)
            return ResultViewModel<LessonViewModel>.Failure("Không tìm thấy bài học");

        var section = await _unitOfWork.Sections.GetByIdAsync(model.SectionId);
        if (section == null)
            return ResultViewModel<LessonViewModel>.Failure("Học phần không tồn tại.");

        existing.LessonName = model.LessonName;
        existing.Description = model.Description;
        existing.Content = model.Content;
        existing.VideoUrl = model.VideoUrl;
        existing.DocumentUrl = model.DocumentUrl;
        existing.Order = model.Order;
        existing.SectionId = model.SectionId;
        existing.UpdatedAt = DateTime.UtcNow;
        existing.IsActive = model.IsActive;
        existing.IsDeleted = model.IsDeleted;

        await _unitOfWork.CompleteAsync();

        model.UpdatedAt = existing.UpdatedAt;

        return ResultViewModel<LessonViewModel>.Success(model, "Cập nhật bài học thành công");
    }

    public async Task<ResultViewModel<bool>> DeleteLessonAsync(int id)
    {
        var lesson = await _unitOfWork.Lessons.GetByIdAsync(id);
        if (lesson == null)
            return ResultViewModel<bool>.Failure("Không tìm thấy bài học");

        _unitOfWork.Lessons.Remove(lesson);
        await _unitOfWork.CompleteAsync();

        return ResultViewModel<bool>.Success(true, "Xóa bài học thành công");
    }
}