using SchoolManageAPI.Dtos.Account;
using SchoolManageAPI.Models;

namespace SchoolManageAPI.Interfaces.Class;

public interface IAccountRepository {
    public Task Login(LoginDto loginDto);
    public Task<User> SaveRefreshToken(string refreshToken, string idUser);
}