using SchoolManageAPI.Dtos.Class;
using SchoolManageAPI.Models;

namespace SchoolManageAPI.Interfaces.Class;

public interface IClassRepository {
    public Task<Models.Class> CreateAsync(CreateClassDto newClass);
    public Task<List<ClassDto?>> GetAllAsync();
    public Task<ClassDto?> GetByIdAdminAsync(string? id);
    public Task<ClassDto?> GetByIdTeacherAsync(string? id, string leadId);
    public Task<Models.Class?> UpdateAsync(string id,CreateClassDto createClassDto);
    public Task<Models.Class?> DeleteAsync(string id);
    public Task<Models.Class?> CheckNameClass(string name);

}