namespace SchoolManageAPI.Dtos.User;

public class UserDto {
    public string Id { get; set; }
    public string? Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public int YearOfBirth { get; set; } 
  
}

public class TeacherDto : UserDto {
    public string? ClassName { get; set; }   = string.Empty;
}

public class StudentDto : UserDto {
    public string TableId { get; set; }
    public string? TeacherName { get; set; }  = string.Empty;
    public string? ClassName { get; set; }   = string.Empty;
}