using System.ComponentModel.DataAnnotations;

namespace SchoolManageAPI.Dtos.User;

public class UpdateUserDto {
    [MaxLength(100)]
    public string? Name { get; set; } = string.Empty;
    
    public string Phone { get; set; } = string.Empty;
    
    [MaxLength(100)]
    public string Address { get; set; } = string.Empty;
    public int YearOfBirth { get; set; } 
    
    public string? ClassId { get; set; }
    
}