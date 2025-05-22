using Mio.ELearning.Core.ViewModels;

namespace Mio.ELearning.Service.Services;

public interface ISectionService
{
    Task<ResultViewModel<List<SectionViewModel>>> GetAllSectionsAsync(int courseId);
    Task<ResultViewModel<SectionViewModel>> GetSectionByIdAsync(int id);
    Task<ResultViewModel<SectionViewModel>> AddSectionAsync(SectionViewModel model);
    Task<ResultViewModel<SectionViewModel>> UpdateSectionAsync(SectionViewModel model);
    Task<ResultViewModel<bool>> DeleteSectionAsync(int id);
}