using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Mio.Elearning.Core.Models;

namespace Mio.ELearning.Core.Models;

// [Index(nameof(UserId), nameof(QuizId), IsUnique = true)]
public class Result : BaseModel
{
    [Key]
    public int ResultId { get; set; }
    public int UserId { get; set; }
    [ForeignKey("UserId")]
    public User User { get; set; }
    public int QuizId { get; set; }
    [ForeignKey("QuizId")]
    public Quiz Quiz { get; set; }
    [Range(0, 100)]
    public int Score { get; set; }
    public DateTime CompletedAt { get; set; }
    public bool IsCompleted { get; set; }
}