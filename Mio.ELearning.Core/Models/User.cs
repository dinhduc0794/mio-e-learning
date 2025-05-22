using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Mio.Elearning.Core.Models;

namespace Mio.ELearning.Core.Models;

public class User : BaseModel
{
    [Key]
    public int UserId { get; set; }
    [Required, MaxLength(50)]
    public string UserName { get; set; }
    [Required, MaxLength(100)]
    public string Password { get; set; }
    [Required, EmailAddress, MaxLength(100)]
    public string Email { get; set; }
    [Required, MaxLength(100)]
    public string FullName { get; set; }
    [MaxLength(15)]
    public string PhoneNumber { get; set; }
    public DateTime? Dob { get; set; }
    [MaxLength(200)]
    public string Address { get; set; }
    [MaxLength(500)]
    public string Image { get; set; }
    public int RoleId { get; set; }
    [ForeignKey("RoleId")]
    public Role Role { get; set; }
    public ICollection<Enrollment> Enrollments { get; set; }
    public ICollection<Feedback> Feedbacks { get; set; }
    public ICollection<Submission> Submissions { get; set; }
    public ICollection<UserAnswer> UserAnswers { get; set; }
}