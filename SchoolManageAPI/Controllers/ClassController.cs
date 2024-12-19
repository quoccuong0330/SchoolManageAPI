using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManageAPI.Dtos.Class;
using SchoolManageAPI.Interfaces.Class;
using SchoolManageAPI.Models;
using WebAPI.Interfaces;

namespace SchoolManageAPI.Controllers;

[Route("api/class")]
[ApiController]
public class ClassController : ControllerBase{
    private readonly IClassRepository _classRepository;
    private readonly ITokenService _tokenService;

    public ClassController(IClassRepository classRepository, ITokenService tokenService) {
        _classRepository = classRepository;
        _tokenService = tokenService;
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "admin, teacher")]
    public async Task<IActionResult> GetById([FromRoute] string id) {
        var userId = _tokenService.GetIdUserLogin(HttpContext);
        if (userId is null) return BadRequest("Khong tim thay access token");

        if (HttpContext.User.IsInRole("teacher")) {
            var classModel = await _classRepository.GetByIdTeacherAsync(id,userId);
            return classModel != null ? Ok(classModel) : NotFound();
        }else if (HttpContext.User.IsInRole("admin")) {
            var classModel = await _classRepository.GetByIdAdminAsync(id);
            return classModel != null ? Ok(classModel) : NotFound();
        }
        return Forbid("Khong co quyen truy cap");
    }
    
    [HttpGet]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> GetAll() {
        var classModel = await _classRepository.GetAllAsync();
        return Ok(classModel);
    }
    
    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Create([FromBody] CreateClassDto newClass) {
        var checkName = await _classRepository.CheckNameClass(newClass.Name);
        if (checkName != null) return BadRequest("Name has exists");
        var classModel = await _classRepository.CreateAsync(newClass);
        return CreatedAtAction(nameof(GetById), new {id = classModel.Id}, classModel);
    }
    
    [HttpPut("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] CreateClassDto newClass) {
        var checkName = await _classRepository.CheckNameClass(newClass.Name);
        if (checkName != null) return BadRequest("Name has exists");
        var classModel = await _classRepository.UpdateAsync(id,newClass);
        return CreatedAtAction(nameof(GetById), new {id = classModel.Id}, classModel);
    }
    
    [HttpDelete("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> DeleteById([FromRoute] string id) {
        var classModel = await _classRepository.DeleteAsync(id);
        return Ok(classModel);
    }
    
    
}