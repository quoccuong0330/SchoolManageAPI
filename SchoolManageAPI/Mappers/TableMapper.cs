using SchoolManageAPI.Dtos.Table;
using SchoolManageAPI.Models;

namespace SchoolManageAPI.Mappers;

public static class TableMapper {
    public static TableDto covertTabletoTableDto(this TablePoint tablePoint) {
        return new TableDto {
            Math = tablePoint.Math,
            Chemistry = tablePoint.Chemistry,
            Physical = tablePoint.Physical,
            Average = tablePoint.Average,
            StudentName = tablePoint.Student?.Name,
            ClassName = tablePoint.Student?.Class?.Name
        };
    }
}