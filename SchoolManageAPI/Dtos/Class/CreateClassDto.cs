using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SchoolManageAPI.Models;

namespace SchoolManageAPI.Dtos.Class;

public class CreateClassDto {
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
}