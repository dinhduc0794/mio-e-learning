using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Mio.Elearning.Core.Models;

namespace Mio.ELearning.Core.Models;

public class Section : BaseModel
{
    [Key]
    public int SectionId { get; set; }
    [Required, MaxLength(100)]
    public string SectionName { get; set; }
    public string? Description { get; set; }
    public int Order { get; set; }
    public int CourseId { get; set; }
    [ForeignKey("CourseId")]
    public Course Course { get; set; }
    public ICollection<Lesson> Lessons { get; set; }
    public ICollection<Quiz> Quizzes { get; set; }
}
