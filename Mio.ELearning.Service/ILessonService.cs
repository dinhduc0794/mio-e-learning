using Mio.ELearning.Core.ViewModels;
using System.Threading.Tasks;

namespace Mio.ELearning.Service.Services;

public interface ILessonService
{
    Task<ResultViewModel<List<LessonViewModel>>> GetAllLessonsAsync(int? sectionId);
    Task<ResultViewModel<LessonViewModel>> GetLessonByIdAsync(int id);
    Task<ResultViewModel<LessonViewModel>> AddLessonAsync(LessonViewModel lessonViewModel);
    Task<ResultViewModel<LessonViewModel>> UpdateLessonAsync(LessonViewModel lessonViewModel);
    Task<ResultViewModel<bool>> DeleteLessonAsync(int id);
}