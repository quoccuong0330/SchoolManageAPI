using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SchoolManageAPI.Data;
using SchoolManageAPI.Dtos.User;
using SchoolManageAPI.Interfaces.Class;
using SchoolManageAPI.Mappers;
using SchoolManageAPI.Models;
using WebAPI.Interfaces;

namespace SchoolManageAPI.Repositories;

public class UserRepository : IUserRepository {
    private readonly ApplicationDbContext _context;
    private readonly ITokenService _tokenService;

    public UserRepository(ApplicationDbContext context, ITokenService tokenService) {
        _context = context;
        _tokenService = tokenService;
    }

    public async Task<UserDto?> GetByIdStudentAsync(string id, string userId) {
        var userModel= await _context.Users
            .Include(x=>x.Class)
            .Include(x=>x.Class.Lead)
            .FirstOrDefaultAsync(u=>u.Id == id);
        if (userModel == null) return null;
        return !userModel.Id.Equals(userId) ? null : userModel?.ConvertUserToDto();
    }

    public async Task<UserDto?> GetByIdTeacherAsync(string id, string userId) {
        var userModel= await _context.Users
            .Include(x=>x.Class)
            .Include(x=>x.Class.Lead)
            .FirstOrDefaultAsync(u=>u.Id == id);
        if (userModel == null) return null;
        if (!userModel.Id.Equals(userId) || !userModel.Class.LeadId.Equals(userId)) return null;
        return  userModel?.ConvertUserToDto();
    }

    public async Task<UserDto?> GetByIdAdminAsync(string id) {
         var userModel= await _context.Users
             .Include(x=>x.Class)
             .Include(x=>x.Class.Lead)
             .FirstOrDefaultAsync(u=>u.Id == id);
         return userModel?.ConvertUserToDto();
    }

    public async Task<List<UserDto?>> GetAllAsync() {
        var arrayModelUser=  await _context.Users
            .Include(x=>x.Class)
            .Include(x=>x.Class.Lead)
            .ToListAsync();
        List<UserDto?> users = [];
        users.AddRange(arrayModelUser.Select(user => user.ConvertUserToDto()));
        return users;
    }

    public async Task<UserDto?> CreateAsync(CreateUserDto createUserDto) {
        var user = createUserDto.CreateDtoToUser();
        if (user.Role.ToLower().Trim() == "teacher") {
            var userModel = await _context.Users.AddAsync(user);
        
            await _context.SaveChangesAsync();

            if (userModel?.Entity == null) return null;
            if (userModel.Entity?.ClassId == null) return userModel?.Entity?.ConvertUserToDto();
            var classModel = await _context.Classes
                .Include(x=>x.Lead)
                .Include(x=>x.Students)
                .FirstOrDefaultAsync(x => userModel.Entity.ClassId.Equals(x.Id));
            if (classModel != null) {
                userModel.Entity.ClassId = classModel.Id;
                classModel.Lead = userModel.Entity;
                classModel.LeadId = userModel.Entity.Id;
            }
            await _context.SaveChangesAsync();
            return userModel.Entity.ConvertUserToDto();
        } else if (user.Role.ToLower().Trim() == "student") {
            var userModel = await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            if (userModel?.Entity == null) return null;

            var classModel = await _context.Classes
                .Include(x => x.Lead)
                .Include(x => x.Students)
                .FirstOrDefaultAsync(x => x.Id == userModel.Entity.ClassId);

            if (classModel == null) {
                throw new Exception("Class not found for the given ClassId.");
            }

            classModel.Students.Add(userModel.Entity);
            await _context.SaveChangesAsync(); 

            var tableData = new TablePoint {
                Chemistry = 0,
                Math = 0,
                Physical = 0,
                EditorId = classModel.LeadId, 
                StudentId = userModel.Entity.Id  
            };

            var modelTable = await _context.Tables.AddAsync(tableData);
            await _context.SaveChangesAsync();

            userModel.Entity.TableId = modelTable.Entity.Id;
            await _context.SaveChangesAsync();

            return userModel.Entity.ConvertUserToDto();
        } else if (user.Role.Equals("admin")) {
            var userModel = await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return userModel.Entity.ConvertUserToDto();
        }

        return null;
    }

    public async Task<UserDto?> DeleteAsync(string id) {
        var userModel= await _context.Users
            .Include(x=>x.Class)
            .Include(x=>x.Class.Lead)
            .FirstOrDefaultAsync(u=>u.Id == id);
        if (userModel == null) return null;
        
        var tablePoint = await _context.Tables.FirstOrDefaultAsync(t => id.Equals(t.StudentId));
        if (tablePoint != null) {
            _context.Tables.Remove(tablePoint);
        }
        _context.Remove(userModel);
        await _context.SaveChangesAsync();
        return userModel?.ConvertUserToDto();
    }

    public async Task<UserDto?> UpdateAdminAsync(string id, UpdateUserDto updateUserDto) {
        var checkUser =  await _context.Users
            .Include(x=>x.Class)
            .Include(x=>x.Class!.Lead)
            .FirstOrDefaultAsync(u=>id.Equals(u.Id));
        
        if (checkUser == null) return null;
        // checkUser.Name = updateUserDto.Name.Equals("") ? checkUser.Name:  updateUserDto.Name;
        // checkUser.Phone = updateUserDto.Phone.Equals("") ? checkUser.Phone:  updateUserDto.Phone;
        // checkUser.YearOfBirth = updateUserDto.YearOfBirth == 0 ? checkUser.YearOfBirth:  updateUserDto.YearOfBirth;
        // checkUser.Address = updateUserDto.Address.Equals("") ? checkUser.Address:  updateUserDto.Address;
        if (updateUserDto.ClassId is not null) {
            var checkOldClass = await _context.Classes
                .Include(c => c.Lead)
                .Include(c => c.Students)
                .FirstOrDefaultAsync(x => x.Id.Equals(checkUser.Class!.Id));
            var checkNewClass = await _context.Classes
                .Include(c => c.Lead)
                .Include(c => c.Students)
                .FirstOrDefaultAsync(x => x.Id.Equals(updateUserDto.ClassId));
            if (!checkNewClass.Id.Equals(checkOldClass.Id)) {
                if (checkUser.Role.ToLower().Trim().Equals("student")) {
                    checkUser.ClassId = updateUserDto.ClassId;
                    checkOldClass.Students.Remove(checkUser);
                    checkNewClass.Students.Add(checkUser);
                }
                else if (checkUser.Role.ToLower().Trim().Equals("teacher")) {
                    if (checkNewClass.LeadId is null) {
                        checkOldClass.LeadId = null;
                        checkNewClass.LeadId = checkUser.Id;
                    }
                    else {
                        var idTemp = checkNewClass.LeadId;
                        checkNewClass.LeadId = checkUser.Id;
                        checkOldClass.LeadId = id;
                    }
                }
            }
        }
        else return null;
        
        await _context.SaveChangesAsync();
        var user = await _context.Users.Include(user => user.Class).FirstOrDefaultAsync(x =>  id.Equals(x.Id));
        return user?.ConvertUserToDto();
    }

    public async Task<UserDto?> UpdateTeacherAsync(string id, UpdateUserDto updateUserDto, string teacherId) {
        var checkUser =  await _context.Users
            .Include(x=>x.Class)
            .Include(x=>x.Class.Lead)
            .FirstOrDefaultAsync(u=>id.Equals(u.Id));

        if (checkUser == null) return null;
        if (!checkUser.Id.Equals(teacherId) || !checkUser.Class.LeadId.Equals(teacherId)) return null;
        checkUser.Name = updateUserDto.Name;
        checkUser.Phone = updateUserDto.Phone;
        checkUser.YearOfBirth = updateUserDto.YearOfBirth;
        checkUser.Address = updateUserDto.Address;
        await _context.SaveChangesAsync();
        var user = await _context.Users.Include(user => user.Class).FirstOrDefaultAsync(x =>  id.Equals(x.Id));
        return user?.ConvertUserToDto();
    }

    public async Task<UserDto?> UpdateStudentAsync(string id, UpdateUserDto updateUserDto, string studentId) {
        var checkUser =  await _context.Users
            .Include(x=>x.Class)
            .Include(x=>x.Class.Lead)
            .FirstOrDefaultAsync(u=>id.Equals(u.Id));

        if (checkUser == null) return null;
        if (!checkUser.Id.Equals(studentId) ) return null;
        checkUser.Name = updateUserDto.Name;
        checkUser.Phone = updateUserDto.Phone;
        checkUser.YearOfBirth = updateUserDto.YearOfBirth;
        checkUser.Address = updateUserDto.Address;
        await _context.SaveChangesAsync();
        var user = await _context.Users.Include(user => user.Class).FirstOrDefaultAsync(x =>  id.Equals(x.Id));
        return user?.ConvertUserToDto();
    }

    public async Task<User?> CheckEmailAsync(string email) {
        var model = await _context.Users
            .Include(x=>x.Class)
            .Include(x=>x.Class.Lead)
            .FirstOrDefaultAsync(u=>u.Email == email);
        return model;
    }

    public async Task<User?> GetUserByRefreshTokenAsync(string refreshToken) {
        var model = await _context.Users.FirstOrDefaultAsync(u=> refreshToken.Equals(u.RefreshToken));
        return model;
    }

    public async Task<User?> UpdateRefreshTokenAsync(string id) {
        var model = await _context.Users.FirstOrDefaultAsync(u=> id.Equals(u.Id));
        if (model is null) return null;
        model.RefreshToken = _tokenService.GenerateRefreshToken();
        model.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        await _context.SaveChangesAsync();
        return model;
    }
}