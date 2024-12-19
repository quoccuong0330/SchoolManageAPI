
using SchoolManageAPI.Models;

namespace WebAPI.Interfaces;

public interface ITokenService {
    public string CreateToken(User user);
    public string GenerateRefreshToken();

    public string? GetIdUserLogin(HttpContext httpContext);
}