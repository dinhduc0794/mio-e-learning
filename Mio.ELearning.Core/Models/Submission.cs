using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Mio.Elearning.Core.Models;

namespace Mio.ELearning.Core.Models;

// [Index(nameof(UserId), nameof(AssignmentId), IsUnique = true)]
public class Submission : BaseModel
{
    [Key]
    public int SubmissionId { get; set; }
    public int AssignmentId { get; set; }
    [ForeignKey("AssignmentId")]
    public Assignment Assignment { get; set; }
    public int UserId { get; set; }
    [ForeignKey("UserId")]
    public User User { get; set; }
    [MaxLength(500)]
    public string FilePath { get; set; }
    public DateTime SubmittedAt { get; set; }
    [Range(0, 100)]
    public int? Grade { get; set; }
    [MaxLength(1000)]
    public string Feedback { get; set; }
}