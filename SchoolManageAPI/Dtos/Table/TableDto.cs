using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManageAPI.Dtos.Table;

public class TableDto {
    public double Math { get; set; }
    public double Chemistry { get; set; }
    public double Physical { get; set; }
    
    [NotMapped]
    public double Average { get; set; }

    public string? StudentName { get; set; } = String.Empty;
    public string? ClassName { get; set; } = String.Empty;
   
}