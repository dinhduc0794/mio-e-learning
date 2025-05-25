using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Mio.Elearning.Core.Models;

namespace Mio.ELearning.Core.ViewModels;

public class LessonViewModel : BaseModel
{
    public int LessonId { get; set; }

    [Required(ErrorMessage = "Tên bài học là bắt buộc")]
    [MaxLength(100)]
    public string LessonName { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }

    [MaxLength(2000)]
    public string? Content { get; set; }

    [MaxLength(500)]
    public string? VideoUrl { get; set; }

    [MaxLength(500)]
    public string? DocumentUrl { get; set; }
    
    [NotMapped]
    public IFormFile? VideoFile { get; set; }

    [NotMapped]
    public IFormFile? DocumentFile { get; set; }

    public int Order { get; set; }
    public int SectionId { get; set; }
    public string? SectionName { get; set; }
}
