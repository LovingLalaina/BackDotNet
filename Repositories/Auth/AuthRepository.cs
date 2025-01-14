using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using back_dotnet.Models.Domain;
using back_dotnet.Models.DTOs.Users;
using back_dotnet.Repositories.Users;
using Microsoft.AspNetCore.Identity;
using back_dotnet.Exceptions;
using Microsoft.EntityFrameworkCore;
using back_dotnet.Services.Token;
using back_dotnet.Services.Email;

namespace back_dotnet.Repositories.Auth
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IUserRepository _userRepository;
        private readonly HairunSiContext _dbContext;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;

        public AuthRepository(IUserRepository userRepository,HairunSiContext dbContext, ITokenService tokenService, IEmailService emailService)
        {
            _userRepository = userRepository;
            _dbContext = dbContext;
            _tokenService = tokenService;
            _emailService = emailService;
        }
        public async Task<User?> FindByUsernameAsync(string email)
        {
            GetUserDto? identityuser =  await _userRepository.UserWithThisEmail(email);

            if (identityuser != null)
            {
                return new User
                {
                    Email = identityuser.Email,
                    Password = identityuser.Password,
                    Uuid = identityuser.Uuid
                };
            }

            return null;
        }
        
        public async Task<string> recoveryPassword( string email )
        {
            User? user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user is null)
            {
                throw new HttpException(StatusCodes.Status404NotFound, "Le mail n'existe pas");
            }
            try
            {
                string token = await _tokenService.genererTokenResetPassword(user);
                string? emailContent = _emailService.TemplateResetPassword(user, token);
                await _emailService.SendEmailAsync( email , "Récupération de mot de passe", emailContent);

                string tokenSimple = await _tokenService.genererTokenClassic( user );
                return tokenSimple;
            }
            catch( Exception ex )
            {
                throw new HttpException(StatusCodes.Status500InternalServerError, ex.Message );
            }
        }

        public async Task SendNotificationPassword(string username, string email, string password)
        {
            try
            {
                string? emailContent = _emailService.TemplateSendPassword(username, password);
                await _emailService.SendEmailAsync( email , "Mot de passe", emailContent);
            }
            catch(Exception)
            {
                throw new HttpException(StatusCodes.Status400BadRequest, "Impossible d'envoyer le mail");
            }
        }
    }
}