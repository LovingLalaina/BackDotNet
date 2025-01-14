using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using back_dotnet.Models.Domain;
using back_dotnet.Models.DTOs.Auth;
using back_dotnet.Repositories.Auth;
using back_dotnet.Services.Token;
using back_dotnet.Utils;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using back_dotnet.Exceptions;
using back_dotnet.Models.DTOs.Users;
using back_dotnet.Repositories.Users;
using System.ComponentModel.DataAnnotations;
using back_dotnet.Repositories.Users;
using back_dotnet.Utils;
using back_dotnet.ErrorsHandler;




namespace back_dotnet.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;

        public AuthService(IAuthRepository authRepository, ITokenService tokenService, IConfiguration configuration, IUserRepository userRepository)
        {
            _authRepository = authRepository;
            _tokenService = tokenService;
            _configuration = configuration;
            _userRepository = userRepository;
        }
        public async Task<string> LoginAsync(LoginRequestDto loginDto)
        {
            User? user = await _authRepository.FindByUsernameAsync(loginDto.Email);

            if (user == null)
            {
                throw new HttpException(StatusCodes.Status404NotFound, "L'utilisateur est introuvable");
            }

            bool isMatch = Hash.ComparePassword(loginDto.Password, user.Password);

            if (!isMatch)
            {
                throw new HttpException(StatusCodes.Status401Unauthorized, "Mot de passe incorrect");
            }

            string token;
            try
            {
                token = await _tokenService.GenerateToken(user, UtilsMethod.ConvertDayToHour(loginDto.Duration));
            }
            catch (NotSupportedException formatError)
            {
                throw new HttpException(StatusCodes.Status400BadRequest, formatError.Message);
            }

            return token;
        }

        public async Task<string> recoveryPassword(RecoveryPasswordDto recoveryPasswordDto)
        {
            string token = await _authRepository.recoveryPassword(recoveryPasswordDto.Email);

            return token;
        }

        public async Task SendNotificationPassword(string username, string email, string password)
        {
            await _authRepository.SendNotificationPassword(username, email, password);
        }

        public async Task<bool> VerifyResendMail(string token)
        {
            try
            {
                string email = _tokenService.GetMail(token);
                GetUserDto? user = await _userRepository.UserWithThisEmail(email);
                if (user == null) return false;
                string secretKey = $"{_configuration["JWTSettings:KeyResendMail"]}{user.Password}";
                _tokenService.DecodeToken(token, secretKey, true);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> VerifyURLPassword(string token)
        {
            try
            {
                string email = _tokenService.GetMail(token);
                GetUserDto? user = await _userRepository.UserWithThisEmail(email);
                if (user == null) return false;
                string secretKey = $"{_configuration["JWTSettings:KeyResetPassword"]}{user.Password}";
                _tokenService.DecodeToken(token, secretKey, false);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task ResetUserPassword(ResetPasswordDto resetPasswordDto, string token)
        {
            User? user;
            try
            {
                var payloadUser = _tokenService.GetJwtResetPasswordPayloadAsync(token);
                user = await _userRepository.FindUserByUuidAsync(payloadUser.Uuid);
            }
            catch
            {
                throw new HttpException(StatusCodes.Status500InternalServerError, "Une erreur s'est produite");
            }

            try
            {
                await _tokenService.VerifyJwtResetPasswordTokenAsync(token, user, true);
            }

            catch
            {
                throw new HttpException(StatusCodes.Status410Gone, "Ce lien de réinitialisation du mot de passe est expiré, veuillez réessayer de nouveau");
            }

            var validationContext = new ValidationContext(resetPasswordDto, null, null);
            var validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(resetPasswordDto, validationContext, validationResults, true))
            {
                var errors = string.Join(", ", validationResults.Select(v => v.ErrorMessage));
                throw new HttpException(StatusCodes.Status422UnprocessableEntity, errors);
            }

            if (UtilsMethod.CheckIfPasswordContainPersonalInformation(user, resetPasswordDto.Password))
            {
                var validationErrors = new List<ValidationError>
                {
                    new ValidationError
                    {
                        Property = "password",
                        Constraints = new Dictionary<string, string>
                        {
                            { "containPersonalInformation", "Votre mot de passe ne doit pas contenir vos informations personelles" }
                        }
                    }
                };
                throw new UnprocessableEntityException(StatusCodes.Status422UnprocessableEntity, validationErrors);
            }

            if (resetPasswordDto.Password != resetPasswordDto.ConfirmPassword)
            {
                throw new HttpException(StatusCodes.Status403Forbidden, "Le mot de passe ne correspond pas");
            }

            try
            {
                user.Password = Hash.HashPassword(resetPasswordDto.Password);
                await _userRepository.UpdateUserAsync(user.Uuid, user.Password);
            }
            catch
            {
                throw new HttpException(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}