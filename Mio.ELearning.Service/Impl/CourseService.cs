using System.Linq;
using System.Threading.Tasks;
using Mio.ELearning.Core.Models;
using Mio.ELearning.Core.UnitOfWorks;
using Mio.ELearning.Core.ViewModels;

namespace Mio.ELearning.Service.Services.Impl;

public class CourseService : ICourseService
{
    private readonly IUnitOfWork _unitOfWork;

    public CourseService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ResultViewModel<List<CourseViewModel>>> GetAllCoursesAsync(int? categoryId = null)
    {
        var courses = await _unitOfWork.Courses.GetAllWithCategoryAsync();
        var query = courses.AsQueryable();

        if (categoryId.HasValue && categoryId.Value > 0)
        {
            query = query.Where(c => c.CategoryId == categoryId.Value);
        }

        var result = query.Select(c => new CourseViewModel
        {
            CourseId = c.CourseId,
            CourseName = c.CourseName,
            Description = c.Description,
            CategoryId = c.CategoryId,
            CategoryName = c.Category != null ? c.Category.Name : "N/A",
            ImageUrl = c.ImageUrl,
            CreatedAt = c.CreatedAt,
            UpdatedAt = c.UpdatedAt,
            IsActive = c.IsActive,
            IsDeleted = c.IsDeleted,
            CreatedByUserId = c.CreatedByUserId,
            UpdatedByUserId = c.UpdatedByUserId
        }).ToList();

        return ResultViewModel<List<CourseViewModel>>.Success(result, "Lấy danh sách khóa học thành công");
    }

    public async Task<ResultViewModel<CourseViewModel>> GetCourseByIdAsync(int id)
    {
        var course = await _unitOfWork.Courses.GetByIdWithSectionsAndLessonsAsync(id);
        if (course == null)
            return ResultViewModel<CourseViewModel>.Failure("Không tìm thấy khóa học");

        var model = new CourseViewModel
        {
            CourseId = course.CourseId,
            CourseName = course.CourseName,
            Description = course.Description,
            CategoryId = course.CategoryId,
            ImageUrl = course.ImageUrl,
            CreatedAt = course.CreatedAt,
            UpdatedAt = course.UpdatedAt,
            IsActive = course.IsActive,
            IsDeleted = course.IsDeleted,
            CreatedByUserId = course.CreatedByUserId,
            UpdatedByUserId = course.UpdatedByUserId,
            Sections = course.Sections?
                .OrderBy(s => s.Order) 
                .Select(s => new SectionViewModel
                {
                    SectionId = s.SectionId,
                    SectionName = s.SectionName,
                    Description = s.Description,
                    Order = s.Order,
                    Lessons = s.Lessons?
                        .OrderBy(l => l.Order) 
                        .Select(l => new LessonViewModel
                        {
                            LessonId = l.LessonId,
                            LessonName = l.LessonName,
                            Description = l.Description,
                            Content = l.Content,
                            VideoUrl = l.VideoUrl,
                            DocumentUrl = l.DocumentUrl,
                            Order = l.Order
                        }).ToList() ?? new()
                }).ToList() ?? new()
        };

        return ResultViewModel<CourseViewModel>.Success(model, "Lấy chi tiết khóa học thành công");
    }


    public async Task<ResultViewModel<CourseViewModel>> AddCourseAsync(CourseViewModel model)
    {
        if (model.CategoryId.HasValue)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(model.CategoryId.Value);
            if (category == null)
                return ResultViewModel<CourseViewModel>.Failure("Danh mục không tồn tại.");
        }

        if (model.Sections != null)
        {
            foreach (var section in model.Sections)
            {
                if (section.Lessons != null && section.Lessons.Any(l => string.IsNullOrEmpty(l.LessonName)))
                {
                    return ResultViewModel<CourseViewModel>.Failure("Tất cả bài học phải có tên.");
                }
            }
        }

        var course = new Course()
        {
            CourseName = model.CourseName,
            Description = model.Description,
            CategoryId = model.CategoryId,
            ImageUrl = model.ImageUrl, // store file path
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            IsActive = true,
            IsDeleted = false,
            Sections = model.Sections?.Select(s => new Section
            {
                SectionName = s.SectionName,
                Description = s.Description,
                Order = s.Order,
                Lessons = s.Lessons?.Select(l => new Lesson
                {
                    LessonName = l.LessonName,
                    Description = l.Description,
                    Content = l.Content,
                    VideoUrl = l.VideoUrl, // store file path
                    DocumentUrl = l.DocumentUrl, // store file path
                    Order = l.Order
                }).ToList() ?? new List<Lesson>()
            }).ToList() ?? new List<Section>()
        };

        await _unitOfWork.Courses.AddAsync(course);
        await _unitOfWork.CompleteAsync();

        model.CourseId = course.CourseId;
        model.CreatedAt = course.CreatedAt;

        return ResultViewModel<CourseViewModel>.Success(model, "Tạo khóa học thành công");
    }

    public async Task<ResultViewModel<CourseViewModel>> UpdateCourseAsync(CourseViewModel model)
    {
        var existing = await _unitOfWork.Courses.GetByIdWithSectionsAndLessonsAsync(model.CourseId);
        if (existing == null)
            return ResultViewModel<CourseViewModel>.Failure("Không tìm thấy khóa học");

        if (model.CategoryId.HasValue)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(model.CategoryId.Value);
            if (category == null)
                return ResultViewModel<CourseViewModel>.Failure("Danh mục không tồn tại.");
        }

        existing.CourseName = model.CourseName;
        existing.Description = model.Description;
        existing.CategoryId = model.CategoryId;
        existing.ImageUrl = model.ImageUrl; // Updated file path
        existing.UpdatedAt = DateTime.Now;
        existing.IsActive = model.IsActive;
        existing.IsDeleted = model.IsDeleted;
        existing.UpdatedByUserId = model.UpdatedByUserId;

        _unitOfWork.Sections.RemoveRange(existing.Sections);
        existing.Sections = model.Sections?.Select(s => new Section
        {
            SectionId = s.SectionId,
            SectionName = s.SectionName,
            Description = s.Description,
            Order = s.Order,
            Lessons = s.Lessons?.Where(l => !string.IsNullOrEmpty(l.LessonName)).Select(l => new Lesson
            {
                LessonId = l.LessonId,
                LessonName = l.LessonName,
                Description = l.Description,
                Content = l.Content,
                VideoUrl = l.VideoUrl, // updated file path
                DocumentUrl = l.DocumentUrl, // update file path
                Order = l.Order
            }).ToList() ?? new List<Lesson>()
        }).ToList() ?? new List<Section>();

        await _unitOfWork.CompleteAsync();

        return ResultViewModel<CourseViewModel>.Success(model, "Cập nhật khóa học thành công");
    }

    public async Task<ResultViewModel<bool>> DeleteCourseAsync(int id)
    {
        var course = await _unitOfWork.Courses.GetByIdAsync(id);
        if (course == null)
            return ResultViewModel<bool>.Failure("Không tìm thấy khóa học");

        _unitOfWork.Courses.Remove(course);
        await _unitOfWork.CompleteAsync();

        return ResultViewModel<bool>.Success(true, "Xóa khóa học thành công");
    }
}