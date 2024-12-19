using SchoolManageAPI.Dtos.Class;
using SchoolManageAPI.Models;

namespace SchoolManageAPI.Mappers;

public static class ClassMapper {
    public static Class CreateDtoToClass(this CreateClassDto createClassDto) {
        return new Class {
            Name = createClassDto.Name,
            Students = [],
            LeadId = null,
            Lead = null,
        };
    }
    
    public static ClassDto CovertClassToClassDto(this Class ClassCovert) {
        return new ClassDto {
            Id = ClassCovert.Id,
            Name = ClassCovert.Name,
            LeadName = ClassCovert.Lead?.Name,
            Students = ClassCovert.Students.Select(x=>x.ConvertUserToDto()),
            StudentCount = ClassCovert.StudentCount
        };
    }
}