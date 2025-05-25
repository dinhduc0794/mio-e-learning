using System.ComponentModel.DataAnnotations;
using Mio.Elearning.Core.Models;

namespace Mio.ELearning.Core.ViewModels;

public class SectionViewModel : BaseModel
{
    public int SectionId { get; set; }

    public string SectionName { get; set; }
    public string Description { get; set; }
    public int Order { get; set; }
    public int CourseId { get; set; }
    public string? CourseName { get; set; }

    public List<LessonViewModel> Lessons { get; set; } = new List<LessonViewModel>();
}