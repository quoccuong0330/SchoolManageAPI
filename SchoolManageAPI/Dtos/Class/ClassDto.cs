using SchoolManageAPI.Dtos.User;

namespace SchoolManageAPI.Dtos.Class;

public class ClassDto {
    public string Id { get; set; }
    public string Name { get; set; }
    public string? LeadName { get; set; }
    public IEnumerable<UserDto> Students { get; set; }
    public int StudentCount{ get; set; }
}