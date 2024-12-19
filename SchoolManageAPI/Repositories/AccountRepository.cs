using Microsoft.EntityFrameworkCore;
using SchoolManageAPI.Data;
using SchoolManageAPI.Dtos.Account;
using SchoolManageAPI.Interfaces.Class;
using SchoolManageAPI.Models;

namespace SchoolManageAPI.Repositories;

public class AccountRepository : IAccountRepository {
    private readonly ApplicationDbContext _context;

    public AccountRepository(ApplicationDbContext context) {
        _context = context;
    }
    
    public Task Login(LoginDto loginDto) {
        throw new NotImplementedException();
    }

    public async Task<User> SaveRefreshToken(string refreshToken, string idUser) {
        var modelUser = await _context.Users.FirstOrDefaultAsync(x => idUser.Equals(x.Id));
        if (modelUser == null) return null;
        modelUser.RefreshToken = refreshToken;
        modelUser.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        await _context.SaveChangesAsync();
        return modelUser;
    }
}