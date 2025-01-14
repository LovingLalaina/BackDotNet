using System.Security.Claims;
using System.Text;
using back_dotnet.Models.Domain;
using Microsoft.IdentityModel.Tokens;
using back_dotnet.Utils;
using System.IdentityModel.Tokens.Jwt;
using back_dotnet.Repositories.Users;
using back_dotnet.Models.DTOs.Users;
using Newtonsoft.Json;

namespace back_dotnet.Services.Token;
public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    private readonly IUserRepository _userRepository;

    public TokenService(IConfiguration configuration, IUserRepository userRepository)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public async Task<string> GenerateToken(User user, int duration)
    {
        string email = user.Email;

        GetUserDto? getUserDto = await _userRepository.GetUserWithMail(email)
;

        var claims = new[]
        {
            new Claim("user", JsonConvert.SerializeObject(getUserDto, Formatting.None))
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Tokens.TOKEN_KEY));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: Tokens.TOKEN_KEY,
            audience: Tokens.TOKEN_KEY,
            claims: claims,
            expires: DateTime.Now.AddHours(duration),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public ClaimsPrincipal ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(Tokens.SECRET_KEY_TOKEN_RESEND_MAIL);

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,  // Valide la clé de signature
            IssuerSigningKey = new SymmetricSecurityKey(key)
, // Utilise la même clé pour vérifier la signature
            ValidateIssuer = false,          // Ne valide pas l'émetteur (peut être activé si nécessaire)
            ValidateAudience = false,        // Ne valide pas le public (peut être activé si nécessaire)
            ClockSkew = TimeSpan.Zero        // Pas de tolérance de décalage horaire (0 secondes)
        };

        try
        {
            // Valide le token et retourne les revendications associées (claims)
            var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            return principal;
        }
        catch
        {
            // Lance une exception si le token est invalide ou a expiré
            throw new UnauthorizedAccessException("Token non valide");
        }
    }

    public ClaimsPrincipal DecodeToken(string token, string secretKey, bool ignoreExpiration)
    {
        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        byte[] key = System.Text.Encoding.UTF8.GetBytes(secretKey);

        try
        {
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero,
                ValidateLifetime = !ignoreExpiration
            };
            ClaimsPrincipal principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

            if (ignoreExpiration)
                validationParameters.LifetimeValidator = (notBefore, expires, securityToken, validationParameters) =>
                {
                    return expires == null || expires > DateTime.UtcNow;
                };

            return principal;
        }
        catch (Exception ex)
        {
            throw new SecurityTokenException("Token invalide ou expiré.", ex);
        }
    }

    public Task<string> genererTokenResetPassword(User user)
    {
        return Task.Run(() =>
        {
            try
            {
                var secretKey = $"{_configuration["JWTSettings:KeyResetPassword"]}{user.Password}";
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                {
                    new Claim("uuid", user.Uuid.ToString()),
                    new Claim("email", user.Email)
                };

                var tokenOptions = new JwtSecurityToken(
                    issuer: _configuration["JWTSettings:Issuer"],
                    audience: _configuration["JWTSettings:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(double.Parse(_configuration["JWTSettings:Duration"])),
                    signingCredentials: signingCredentials
                );

                var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                return token;
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de la generation de Token pour Reset Password", ex);
            }
        });
    }

    public Task<string> genererTokenClassic(User user)
    {
        return Task.Run(() =>
        {
            try
            {
                var secretKey = $"{_configuration["JWTSettings:KeyResendMail"]}{user.Password}";
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                {
                    new Claim("email", user.Email)
                };

                var tokenOptions = new JwtSecurityToken(
                    claims: claims,
                    signingCredentials: signingCredentials
                );

                var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                return token;
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de la generation de Simple Token pour Reset Password", ex);
            }
        });
    }

    public string GetMail(string token)
    {
        try
        {
            string? email = new JwtSecurityTokenHandler().ReadJwtToken(token).Claims.FirstOrDefault(c => c.Type == "email")?.Value;
            if (email == null)
                throw new Exception("Token invalide");
            return email;
        }
        catch (Exception)
        {
            throw new Exception("Token invalide");
        }
    }
    public (string? Uuid, string? Email) GetJwtResetPasswordPayloadAsync(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            var jwtToken = tokenHandler.ReadJwtToken(token);
            if (jwtToken == null)
                throw new ArgumentException("Token invalide");

            var uuid = jwtToken.Claims.FirstOrDefault(c => c.Type == "uuid")?.Value;
            var email = jwtToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value;

            if (uuid == null)
                throw new ArgumentException("Identifiant non trouvé dans le token");

            return (uuid, email);
        }
        catch (Exception ex)
        {
            throw new Exception("Erreur lors du décodage du token", ex);
        }
    }

    public async Task<(string Uuid, string Email)> VerifyJwtResetPasswordTokenAsync(string token, User user, bool ignoreJwtTimeout = false)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(Tokens.RESET_PASSWORD_PRIVATE_KEY + user.Password);

        try
        {
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };

            if (ignoreJwtTimeout)
            {
                validationParameters.ValidateLifetime = false;
            }
            else
            {
                validationParameters.ValidateLifetime = true;
            }

            var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

            string? uuid = principal.Claims.FirstOrDefault(c => c.Type == "uuid")?.Value;

            string? email = principal.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;

            if (uuid == null || email == null)
            {
                throw new ArgumentException("Les données du token sont invalides");
            }

            return (uuid, email);
        }
        catch (Exception ex)
        {
            throw new Exception("Erreur lors de la vérification du token", ex);
        }
    }

}