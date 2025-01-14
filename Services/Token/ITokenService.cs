using System.Security.Claims;
using back_dotnet.Models.Domain;

namespace back_dotnet.Services.Token;

public interface ITokenService
{
    public Task<string> GenerateToken(User user, int duration);

    public ClaimsPrincipal ValidateToken(string token);

    public ClaimsPrincipal DecodeToken(string token, string secretKey, bool ignoreExpiration);

    public Task<string> genererTokenResetPassword(User user);

    public Task<string> genererTokenClassic(User user);
    
    public string GetMail(string token);

    public (string Uuid, string Email) GetJwtResetPasswordPayloadAsync(string token);

    Task<(string Uuid, string Email)> VerifyJwtResetPasswordTokenAsync(
        string token,
        User user,
        bool ignoreJwtTimeout = false);
}