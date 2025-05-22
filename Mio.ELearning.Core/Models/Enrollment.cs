using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Mio.Elearning.Core.Models;

namespace Mio.ELearning.Core.Models;

// [Index(nameof(UserId), nameof(CourseId), IsUnique = true)]
public class Enrollment : BaseModel
{
    [Key]
    public int EnrollmentId { get; set; }
    public DateTime EnrollmentDate { get; set; }
    public bool IsCompleted { get; set; }
    public float Progress { get; set; }
    public int UserId { get; set; }
    [ForeignKey("UserId")]
    public User User { get; set; }
    public int CourseId { get; set; }
    [ForeignKey("CourseId")]
    public Course Course { get; set; }
}