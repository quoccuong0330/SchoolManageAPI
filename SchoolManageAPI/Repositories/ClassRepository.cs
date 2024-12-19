using Microsoft.EntityFrameworkCore;
using SchoolManageAPI.Data;
using SchoolManageAPI.Dtos.Class;
using SchoolManageAPI.Interfaces.Class;
using SchoolManageAPI.Mappers;
using SchoolManageAPI.Models;

namespace SchoolManageAPI.Repositories;

public class ClassRepository : IClassRepository {
    private readonly ApplicationDbContext _context;

    public ClassRepository(ApplicationDbContext context) {
        _context = context;
    }
    public async Task<Class> CreateAsync(CreateClassDto newClass) {
        var model = await _context.Classes.AddAsync(newClass.CreateDtoToClass());
        await _context.SaveChangesAsync();
        return model.Entity ;
    }

    public async Task<List<ClassDto?>> GetAllAsync() {
        var classModel = await _context.Classes
            .Include(c => c.Lead)  
            .Include(c => c.Students)
            .ToListAsync();
        List<ClassDto> model = [];
        model.AddRange(classModel.Select(c => c.CovertClassToClassDto()));
        return model;
    }

    public async Task<Class?> UpdateAsync(string id, CreateClassDto createClassDto) {
        var modelClass = await _context.Classes.FirstOrDefaultAsync(c =>  id.Equals(c.Id));
        if (modelClass != null) {
            var isCheck = await _context.Classes.AnyAsync(c => c.Name.Equals(createClassDto.Name));
            if (!isCheck) {
                modelClass.Name = createClassDto.Name;
                await _context.SaveChangesAsync();
                return modelClass;
            }
        }
        return null;
    }

    public async Task<Class?> DeleteAsync(string id) {
        var modelClass = await _context.Classes.FirstOrDefaultAsync(c =>  id.Equals(c.Id));
        if (modelClass == null) return null;
         _context.Remove(modelClass);
         await _context.SaveChangesAsync();
         return modelClass;
    }

    public  async Task<ClassDto?> GetByIdAdminAsync(string? id) {
        var classModel = await _context.Classes
            .Include(c => c.Lead)  
            .Include(c => c.Students)  
            .FirstOrDefaultAsync(c => c.Id == id);

        if (classModel != null) {
            classModel.Students = classModel.Students
                .Where(u => u.Role.ToLower() == "student")
                .ToList();
        }
        return classModel?.CovertClassToClassDto();
    }

    public async Task<ClassDto?> GetByIdTeacherAsync(string? id, string leadId) {
        var classModel = await _context.Classes
            .Include(c => c.Lead)  
            .Include(c => c.Students)  
            .FirstOrDefaultAsync(c => c.Id == id);

        if (classModel != null) {
            if (leadId.Equals(classModel.LeadId)) {
                classModel.Students = classModel.Students
                    .Where(u => u.Role.ToLower() == "student")
                    .ToList();
            } else return null;
        }
        return classModel?.CovertClassToClassDto();
    }

    public async Task<Class?> CheckNameClass(string name) {
        var modelClass = await _context.Classes.FirstOrDefaultAsync(c => c.Name == name);
        return modelClass;
    }
}