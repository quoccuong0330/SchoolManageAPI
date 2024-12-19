using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using SchoolManageAPI.Dtos.Table;
using SchoolManageAPI.Interfaces.Class;
using WebAPI.Interfaces;

namespace SchoolManageAPI.Controllers;

[ApiController]
[Route("/api/point")]
public class TableController : ControllerBase {
    private readonly ITableRepository _tableRepository;
    private readonly ITokenService _tokenService;

    public TableController(ITableRepository tableRepository, ITokenService tokenService) {
        _tableRepository = tableRepository;
        _tokenService = tokenService;
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetById([FromRoute] string id) {
        var userId = _tokenService.GetIdUserLogin(HttpContext);
        if (userId == null) return Unauthorized("Không tìm thấy nameid trong token.");
        if (HttpContext.User.IsInRole("admin") || HttpContext.User.IsInRole("teacher")) {
            var model = await _tableRepository.GetByIdAsync(id);
            return model == null ? NotFound() : Ok(model);
        } else if(HttpContext.User.IsInRole("student")) {
            var model = await _tableRepository.GetByIdStudentAsync(id,userId);
            return model == null ? Forbid() : Ok(model);
        }
        return Forbid("Khong co quyen truy cap");

    }
    
    [HttpGet]
    [Authorize(Roles = "teacher, admin")]
    public async Task<IActionResult> GetAll() {
        var model = await _tableRepository.GetAllAsync();
        return Ok(model);
    }
    
    [HttpPut("{id}")]
    [Authorize(Roles = "teacher, admin")]
    public async Task<IActionResult> Update([FromRoute] string id, UpdateTableDto updateTableDto) {
        var userId = _tokenService.GetIdUserLogin(HttpContext);
        if (userId == null) return Unauthorized("Không tìm thấy nameid trong token.");

        if (HttpContext.User.IsInRole("teacher")) {
            var model = await _tableRepository.UpdateByTeacherAsync(id,updateTableDto, userId);
            return model == null ? NotFound() : Ok(model);
        }else if (HttpContext.User.IsInRole("admin")) {
            var model = await _tableRepository.UpdateByAdminAsync(id,updateTableDto);
            return model == null ? NotFound() : Ok(model);
        }

        return Forbid("Khong co quyen truy cap");
    }
}