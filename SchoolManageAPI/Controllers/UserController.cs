using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManageAPI.Dtos.User;
using SchoolManageAPI.Interfaces.Class;
using WebAPI.Interfaces;

namespace SchoolManageAPI.Controllers;

[Route("api/user")]
[ApiController]
public class UserController : ControllerBase{
    private readonly IUserRepository _userRepository;
    private readonly IClassRepository _classRepository;
    private readonly ITokenService _tokenService;

    public UserController(IUserRepository userRepository, IClassRepository classRepository, ITokenService tokenService) {
        _userRepository = userRepository;
        _classRepository = classRepository;
        _tokenService = tokenService;
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetById([FromRoute] string id) {
        var userId = _tokenService.GetIdUserLogin(HttpContext);
        if (userId is null) return BadRequest("Khong tim thay access token");

        if (HttpContext.User.IsInRole("student")) {
            var userModel = await _userRepository.GetByIdStudentAsync(id, userId);
            return userModel != null ? Ok(userModel) : NotFound();
        }else if (HttpContext.User.IsInRole("teacher")) {
            var userModel = await _userRepository.GetByIdTeacherAsync(id,userId);
            return userModel != null ? Ok(userModel) : NotFound();
        }else if (HttpContext.User.IsInRole("admin")) {
            var userModel = await _userRepository.GetByIdAdminAsync(id);
            return userModel != null ? Ok(userModel) : NotFound();
        }
        return Forbid("Khong co quyen try cap");
    }
    
    [HttpGet]
    [Authorize(Roles = "admin, teacher")]
    public async Task<IActionResult> GetAll() {
        var userModel = await _userRepository.GetAllAsync();
        return Ok(userModel);
    }
    
    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Create([FromBody] CreateUserDto createUserDto) {
        var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
        if (!ModelState.IsValid) return BadRequest(errors);

                
        var checkEmail = await _userRepository.CheckEmailAsync(createUserDto.Email);
        if (checkEmail?.Email != null) return BadRequest("Email has exists");

        var checkClass = await _classRepository.GetByIdAdminAsync(createUserDto.ClassId ?? "");
        switch (createUserDto.Role.ToLower()) {
            case "student": {
                if (checkClass == null) 
                    return BadRequest("ClassId does not exists.");
                break;
            }
            case "teacher": {
                if (checkClass != null && checkClass.LeadName is not null) 
                    return BadRequest("Class already have teacher");
                break;
            }
            case "admin": {
                break;
            }
            default:
                return BadRequest("User must have role");
        }
        
        var userModel = await _userRepository.CreateAsync(createUserDto);
        return userModel != null? Ok(userModel) : BadRequest("Cant create user!");
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateUserDto updateUserDto) {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var userId = _tokenService.GetIdUserLogin(HttpContext);
        if (userId is null) return BadRequest("Khong tim thay access token");

        if (HttpContext.User.IsInRole("student")) {
            var modelUser = await _userRepository.UpdateStudentAsync(id, updateUserDto, userId);
            return modelUser != null ? Ok(modelUser) : NotFound("Not found user with Id");
        }else if (HttpContext.User.IsInRole("teacher")) {
            var modelUser = await _userRepository.UpdateTeacherAsync(id, updateUserDto, userId);
            return modelUser != null ? Ok(modelUser) : NotFound("Not found user with Id");
        }else if (HttpContext.User.IsInRole("admin")) {
            var modelUser = await _userRepository.UpdateAdminAsync(id, updateUserDto);
            return modelUser != null ? Ok(modelUser) : NotFound("Not found user with Id");
        }
        return Forbid("Khong co quyen try cap");
    
       
    }
    
    [HttpDelete("{id}")]    
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Update([FromRoute] string id) {
        if (!ModelState.IsValid) return BadRequest(ModelState);
    
        var modelUser = await _userRepository.DeleteAsync(id);
        return modelUser != null ? Ok(modelUser) : NotFound("Not found user with Id");
    }

    
}