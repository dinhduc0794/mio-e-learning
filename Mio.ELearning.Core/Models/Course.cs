using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Mio.Elearning.Core.Models;

namespace Mio.ELearning.Core.Models;

public class Course : BaseModel
{
    [Key]
    public int CourseId { get; set; }
    [Required, MaxLength(100)]
    public string CourseName { get; set; }
    [MaxLength(1000)]
    public string Description { get; set; }
    
    public int? CategoryId { get; set; }
    [ForeignKey("CategoryId")] 
    public Category? Category { get; set; }
    
    [MaxLength(500)]
    public string? ImageUrl { get; set; }

    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    public ICollection<Section> Sections { get; set; } = new List<Section>();
    public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
    public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
}