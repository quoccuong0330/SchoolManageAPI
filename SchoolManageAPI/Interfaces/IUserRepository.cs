using SchoolManageAPI.Dtos.User;
using SchoolManageAPI.Models;

namespace SchoolManageAPI.Interfaces.Class;

public interface IUserRepository {
    public Task<UserDto?> GetByIdStudentAsync(string id,string userId);
    public Task<UserDto?> GetByIdTeacherAsync(string id,string userId);
    public Task<UserDto?> GetByIdAdminAsync(string id);
    public Task<List<UserDto?>> GetAllAsync();
    public Task<UserDto?> CreateAsync(CreateUserDto createUserDto);
    public Task<UserDto?> DeleteAsync(string id);
    public Task<UserDto?> UpdateAdminAsync(string id,UpdateUserDto updateUserDto);
    public Task<UserDto?> UpdateTeacherAsync(string id,UpdateUserDto updateUserDto,string teacherId);
    public Task<UserDto?> UpdateStudentAsync(string id,UpdateUserDto updateUserDto, string studentId);
    public Task<User?> CheckEmailAsync(string email);

    public Task<User?> GetUserByRefreshTokenAsync(string refreshToken);

    public Task<User?> UpdateRefreshTokenAsync(string id);
}