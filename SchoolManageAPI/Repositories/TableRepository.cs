using Microsoft.EntityFrameworkCore;
using SchoolManageAPI.Data;
using SchoolManageAPI.Dtos.Table;
using SchoolManageAPI.Interfaces.Class;
using SchoolManageAPI.Mappers;

namespace SchoolManageAPI.Repositories;

public class TableRepository : ITableRepository {
    private readonly ApplicationDbContext _context;

    public TableRepository(ApplicationDbContext context) {
        _context = context;
    }
    
    public async Task<TableDto?> GetByIdAsync(string id) {
        var tableModel = await _context.Tables
            .Include(x=>x.Student).
            Include(x=>x.Student.Class)
            .FirstOrDefaultAsync(x => id.Equals(x.Id));
        return tableModel?.covertTabletoTableDto();
    }

    public async Task<TableDto?> GetByIdStudentAsync(string id, string studentId) {
        var tableModel = await _context.Tables
            .Include(x=>x.Student).
            Include(x=>x.Student.Class)
            .FirstOrDefaultAsync(x => id.Equals(x.Id));
        return tableModel != null && studentId.Equals(tableModel.StudentId) 
            ? tableModel?.covertTabletoTableDto() : null;
    }

    public async Task<TableDto?> UpdateByTeacherAsync(string id, UpdateTableDto updateTableDto, string idEditor) {
        var tableModel = await _context.Tables
            .Include(x=>x.Student).
            Include(x=>x.Student.Class)
            .FirstOrDefaultAsync(x => id.Equals(x.Id));
        if (tableModel == null || tableModel.EditorId is null) return null;
        if (!tableModel.EditorId!.Equals(idEditor)) return null;
        tableModel.Chemistry = updateTableDto.Chemistry;
        tableModel.Math = updateTableDto.Math;
        tableModel.Physical = updateTableDto.Physical;
        await _context.SaveChangesAsync();
        return tableModel.covertTabletoTableDto();
    }

    public async Task<TableDto?> UpdateByAdminAsync(string id, UpdateTableDto updateTableDto) {
        var tableModel = await _context.Tables
            .Include(x=>x.Student).
            Include(x=>x.Student.Class)
            .FirstOrDefaultAsync(x => id.Equals(x.Id));
        if (tableModel == null) return null;
        tableModel.Chemistry = updateTableDto.Chemistry;
        tableModel.Math = updateTableDto.Math;
        tableModel.Physical = updateTableDto.Physical;
        await _context.SaveChangesAsync();
        return tableModel.covertTabletoTableDto();
    }

    public async Task<List<TableDto?>> GetAllAsync() {
        var tablesModel = await _context.Tables
            .Include(x => x.Student).Include(x => x.Student.Class)
            .ToListAsync();
        List<TableDto> list = [];
        list.AddRange(tablesModel.Select(x=>x.covertTabletoTableDto()));
        return list;
    }


   
}