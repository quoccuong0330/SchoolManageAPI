using System.ComponentModel.DataAnnotations;

namespace SchoolManageAPI.Dtos.User;

public class CreateUserDto {
 
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(100)]
    public string Password { get; set; } = string.Empty;
    
    [Required]
    public string Phone { get; set; } = string.Empty;
    
    [Required]
    public string Role { get; set; } =  string.Empty; 
    
    [Required]
    [MaxLength(100)]
    public string Address { get; set; } = string.Empty;
    public int YearOfBirth { get; set; } 
    public string? ClassId { get; set; }  = string.Empty;
}