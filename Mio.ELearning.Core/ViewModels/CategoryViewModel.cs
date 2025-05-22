using Mio.ELearning.Core.Models;

namespace Mio.ELearning.Core.ViewModels;

public class CategoryViewModel : Category
{
    public int RecordCount { get; set; }
    public bool IsEdit { get; set; }
    public bool IsSuccess { get; set; }
}