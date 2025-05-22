using System.ComponentModel.DataAnnotations;

namespace Mio.ELearning.Core.ViewModels;

public class AssignmentViewModel
{
    public int AssignmentId { get; set; }
    [Required(ErrorMessage = "Tiêu đề bài tập là bắt buộc")]
    [MaxLength(100)]
    public string Title { get; set; }

    [MaxLength(1000)]
    public string Description { get; set; }

    [Required(ErrorMessage = "Ngày hết hạn là bắt buộc")]
    public DateTime DueDate { get; set; }

    [Range(0, 100, ErrorMessage = "Điểm tối đa phải từ 0 đến 100")]
    public int MaxGrade { get; set; }

    public int CourseId { get; set; }
}