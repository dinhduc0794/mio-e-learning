using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Mio.Elearning.Core.Models;

namespace Mio.ELearning.Core.Models;

// [Index(nameof(UserId), nameof(CourseId), IsUnique = true)]
public class Feedback : BaseModel
{
    [Key]
    public int FeedbackId { get; set; }
    [Range(1, 5)]
    public int Rating { get; set; }
    [Required, MaxLength(1000)]
    public string Content { get; set; }

    public int UserId { get; set; }
    [ForeignKey("UserId")]
    public User User { get; set; }

    public int CourseId { get; set; }
    [ForeignKey("CourseId")]
    public Course Course { get; set; }
}