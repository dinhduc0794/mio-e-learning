using System.ComponentModel.DataAnnotations;
using Mio.Elearning.Core.Models;

namespace Mio.ELearning.Core.Models;

public class Category : BaseModel
{
    [Key]
    public int CategoryId { get; set; }
    [Required]
    [StringLength(100)]
    public string Name { get; set; }
    public string? Description { get; set; }
    
    public ICollection<Course> Courses { get; set; } = new List<Course>();
}