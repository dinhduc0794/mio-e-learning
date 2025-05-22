using System.ComponentModel.DataAnnotations;

namespace Mio.ELearning.Core.ViewModels;

public class SectionViewModel
{
    public int SectionId { get; set; }

    [Required(ErrorMessage = "Tên phần là bắt buộc")]
    [MaxLength(100)]
    public string SectionName { get; set; }

    [MaxLength(1000)]
    public string Description { get; set; }

    public int Order { get; set; }

    public List<LessonViewModel> Lessons { get; set; } = new List<LessonViewModel>();
}