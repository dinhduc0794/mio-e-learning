using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Mio.ELearning.Core.Models;

namespace Mio.Elearning.Core.Models;

public class Assignment : BaseModel
{
    [Key]
    public int AssignmentId { get; set; }
    [Required, MaxLength(100)]
    public string Title { get; set; }
    [MaxLength(1000)]
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
    [Range(0, 100)]
    public int MaxGrade { get; set; }
    public int CourseId { get; set; }
    [ForeignKey("CourseId")]
    public Course Course { get; set; }
    public ICollection<Submission> Submissions { get; set; }
}