using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Mio.Elearning.Core.Models;

namespace Mio.ELearning.Core.Models;

public class Quiz : BaseModel
{
    [Key]
    public int QuizId { get; set; }
    [Required, MaxLength(100)]
    public string Title { get; set; }
    public int SectionId { get; set; }
    [ForeignKey("SectionId")]
    public Section Section { get; set; }
    public DateTime AvailableFrom { get; set; }
    public DateTime AvailableTo { get; set; }
    public int Duration { get; set; }
    public ICollection<Question> Questions { get; set; }
}