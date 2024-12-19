using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManageAPI.Models;

public class Class {
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString(); // ID mặc định
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    // Foreign Key to Lead
    [ForeignKey("Lead")]
    public string? LeadId { get; set; } // Nullable nếu không phải lớp nào cũng có Lead
    public User? Lead { get; set; }
    
    // Navigation property for Students
    public ICollection<User> Students { get; set; } = new List<User>();
    
    // Computed property
    [NotMapped]
    public int StudentCount => Students?.Where(s => s.Role.ToLower() == "student").Count() ?? 0;
}