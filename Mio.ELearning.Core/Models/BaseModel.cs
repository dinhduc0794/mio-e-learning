using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Mio.ELearning.Core.Models;

namespace Mio.Elearning.Core.Models;

public abstract class BaseModel
{
    public BaseModel()
    {
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
        IsActive = true;
        IsDeleted = false;
    }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int? CreatedByUserId { get; set; }
    [NotMapped]
    public User? CreatedByUser { get; set; } // Không ánh xạ vào database
    public int? UpdatedByUserId { get; set; }
    [NotMapped]
    public User? UpdatedByUser { get; set; } // Không ánh xạ vào database
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
}