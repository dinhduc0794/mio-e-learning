using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Mio.Elearning.Core.Models;

namespace Mio.ELearning.Core.Models;


public class Lesson : BaseModel
{
    [Key]
    public int LessonId { get; set; }
    [Required, MaxLength(100)]
    public string LessonName { get; set; }
    [MaxLength(1000)]
    public string? Description { get; set; }
    [MaxLength(2000)]
    public string? Content { get; set; }
    [MaxLength(500)]
    public string? VideoUrl { get; set; }
    [MaxLength(500)]
    public string? DocumentUrl { get; set; }
    public int Order { get; set; }
    public int SectionId { get; set; }
    [ForeignKey("SectionId")]
    public Section Section { get; set; }
}