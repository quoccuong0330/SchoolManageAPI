using SchoolManageAPI.Dtos.Table;

namespace SchoolManageAPI.Interfaces.Class;

public interface ITableRepository {
    public Task<TableDto?> GetByIdAsync(string id);
    public Task<TableDto?> GetByIdStudentAsync(string id,string studentId);
    public Task<TableDto?> UpdateByTeacherAsync(string id, UpdateTableDto updateTableDto, string idEditor);
    public Task<TableDto?> UpdateByAdminAsync(string id, UpdateTableDto updateTableDto);
    public Task<List<TableDto?>> GetAllAsync();
}