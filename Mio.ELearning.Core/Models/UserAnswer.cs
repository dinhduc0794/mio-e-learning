using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Mio.Elearning.Core.Models;

namespace Mio.ELearning.Core.Models;

// [Index(nameof(UserId), nameof(QuestionId), IsUnique = true)]
public class UserAnswer : BaseModel
{
    [Key]
    public int UserAnswerId { get; set; }
    public int QuestionId { get; set; }
    [ForeignKey("QuestionId")]
    public Question Question { get; set; }
    public int UserId { get; set; }
    [ForeignKey("UserId")]
    public User User { get; set; }
    public int AnswerId { get; set; }
    [ForeignKey("AnswerId")]
    public Answer Answer { get; set; }
}