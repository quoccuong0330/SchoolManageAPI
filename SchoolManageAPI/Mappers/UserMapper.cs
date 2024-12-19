using SchoolManageAPI.Dtos.User;
using SchoolManageAPI.Models;

namespace SchoolManageAPI.Mappers;

public static class UserMapper {
    public static User CreateDtoToUser(this CreateUserDto createUserDto) {
        var passwordEncrypt = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password);
        return new User  {
            Name = createUserDto.Name,
            Email = createUserDto.Email,
            Password = passwordEncrypt,
            Phone = createUserDto.Phone,
            Role = createUserDto.Role,
            Address = createUserDto.Address,
            YearOfBirth = createUserDto.YearOfBirth,
            ClassId = createUserDto.ClassId ?? null,
        };
    }

    public static UserDto ConvertUserToDto(this User user) {
        return user.Role.ToLower().Trim() switch {
            "student" => new StudentDto() {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone,
                Role = user.Role,
                Address = user.Address,
                YearOfBirth = user.YearOfBirth,
                TableId = user.TableId,
                TeacherName = user.Class?.Lead.Name,
                ClassName =  user.Class.Name,
            },
            "teacher" => new TeacherDto() {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone,
                Role = user.Role,
                Address = user.Address,
                YearOfBirth = user.YearOfBirth,
                ClassName = user.ClassId == null ? null : user.Class.Name,
            },
            "admin" => new UserDto {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone,
                Role = user.Role,
            },
            _ => throw new ArgumentOutOfRangeException()
        };
    }

}
    
 
