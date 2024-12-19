using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace SchoolManageAPI.Models;

public class User  {
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString(); // ID mặc định
    
    [Required]
    [MaxLength(100)]
    public string? Name { get; set; } = string.Empty; // Không cần nullable
    
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    public string Password { get; set; } = string.Empty;
    
    [Required]
    [Phone]
    public string Phone { get; set; } = string.Empty;
    
    [Required]
    public string Role { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(100)]
    public string Address { get; set; } = string.Empty;
    
    public int YearOfBirth { get; set; }
    
    // Foreign Key to TablePoint
  
    [ForeignKey("TablePoint")]
    public string? TableId { get; set; }
    [InverseProperty("Student")]
    public TablePoint TablePoint { get; set; }
    
    // Foreign Key to Class
    [ForeignKey("Class")]
    public string? ClassId { get; set; } // Nullable để hỗ trợ "SET NULL"
    public Class? Class { get; set; }
    
    // AccessToken properties
    public string? AccessToken { get; set; }
    public int? ExpiresIn { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiry { get; set; }
}
