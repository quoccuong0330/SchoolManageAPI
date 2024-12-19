using Microsoft.AspNetCore.Mvc;
using SchoolManageAPI.Dtos.Account;
using SchoolManageAPI.Interfaces.Class;
using WebAPI.Interfaces;

namespace SchoolManageAPI.Controllers;

[ApiController]
[Route("/api/account")]
public class AccountController : ControllerBase {
    private readonly IAccountRepository _accountRepository;
    private readonly ITokenService _tokenService;
    private readonly IUserRepository _userRepository;

    public AccountController(IAccountRepository accountRepository,
        ITokenService tokenService, IUserRepository userRepository) {
        _accountRepository = accountRepository;
        _tokenService = tokenService;
        _userRepository = userRepository;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto accountDto) {
        if (!ModelState.IsValid) return BadRequest();
        var user = await _userRepository.CheckEmailAsync(accountDto.Email);
        // if (user == null) return Unauthorized("Invalid email");
        var isPasswordValid = BCrypt.Net.BCrypt.Verify(accountDto.Password, user.Password);
        if (!isPasswordValid) return Unauthorized("Email or password is incorrect");
        var refreshToken = _tokenService.GenerateRefreshToken();
        var userModel = await _accountRepository.SaveRefreshToken(refreshToken, user.Id);
        return Ok(new LoginResponseDto() {
            AccessToken = _tokenService.CreateToken(user),
            RefreshToken = userModel.RefreshToken,
            // RefreshTokenExpiry =userModel.RefreshTokenExpiry
        });
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto) {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        if (string.IsNullOrEmpty(token))
            return BadRequest("Access token không được cung cấp.");

        if (true) {
            var user = await _userRepository.GetUserByRefreshTokenAsync(refreshTokenDto.RefreshToken);

            if (user == null || user.RefreshTokenExpiry <= DateTime.UtcNow) {
                return Unauthorized("Invalid or expired refresh token");
            }

            var newAccessToken = _tokenService.CreateToken(user);

            var model = await _userRepository.UpdateRefreshTokenAsync(user.Id);
            if (model is null) return BadRequest();

            return Ok(new {
                AccessToken = newAccessToken,
                RefreshToken = model.RefreshToken
            });
        }

        return BadRequest();
    }

}