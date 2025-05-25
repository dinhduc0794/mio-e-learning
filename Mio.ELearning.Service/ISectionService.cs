using Mio.ELearning.Core.ViewModels;
using System.Threading.Tasks;

namespace Mio.ELearning.Service.Services;

public interface ISectionService
{
    Task<ResultViewModel<List<SectionViewModel>>> GetAllSectionsAsync(int? courseId);
    Task<ResultViewModel<SectionViewModel>> GetSectionByIdAsync(int id);
    Task<ResultViewModel<SectionViewModel>> AddSectionAsync(SectionViewModel sectionViewModel);
    Task<ResultViewModel<SectionViewModel>> UpdateSectionAsync(SectionViewModel sectionViewModel);
    Task<ResultViewModel<bool>> DeleteSectionAsync(int id);
}