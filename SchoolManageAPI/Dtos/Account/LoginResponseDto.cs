namespace SchoolManageAPI.Dtos.Account;

public class LoginResponseDto {
    public string AccessToken { get; set; } = string.Empty;
    // public int ExpireIn { get; set; }
    public string? RefreshToken { get; set; }
    // public DateTime? RefreshTokenExpiry { get; set; }
    
}