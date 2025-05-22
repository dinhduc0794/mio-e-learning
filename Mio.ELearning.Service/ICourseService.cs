using Mio.ELearning.Core.ViewModels;

namespace Mio.ELearning.Service.Services;

public interface ICourseService
{
    Task<ResultViewModel<List<CourseViewModel>>> GetAllCoursesAsync(int? categoryId);
    Task<ResultViewModel<CourseViewModel>> GetCourseByIdAsync(int id);
    Task<ResultViewModel<CourseViewModel>> AddCourseAsync(CourseViewModel courseViewModel);
    Task<ResultViewModel<CourseViewModel>> UpdateCourseAsync(CourseViewModel courseViewModel);
    Task<ResultViewModel<bool>> DeleteCourseAsync(int id);
}