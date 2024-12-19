using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManageAPI.Models;

public class TablePoint {
    [Key] public string Id { get; set; } = Guid.NewGuid().ToString(); 
    public double Math { get; set; }
    public double Chemistry { get; set; }
    public double Physical { get; set; }
    
    [NotMapped]
    public double Average => (Math + Chemistry + Physical) / 3;  

    [ForeignKey("User")]
    public string StudentId { get; set; }
    [InverseProperty("TablePoint")]
    public User Student { get; set; }

    [ForeignKey("Editor")]
    public string? EditorId { get; set; }  
    public User? Editor { get; set; }
}