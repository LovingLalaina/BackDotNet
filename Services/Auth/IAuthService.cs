using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using back_dotnet.Models.DTOs.Auth;

namespace back_dotnet.Services.Auth
{
    public interface IAuthService
    {
        Task<string> LoginAsync(LoginRequestDto loginDto);
        public Task<string> recoveryPassword( RecoveryPasswordDto recoveryPasswordDto );
        public Task SendNotificationPassword(string username, string email, string password);
        public Task<bool> VerifyResendMail(string token);
        public Task<bool> VerifyURLPassword(string token);
        public Task ResetUserPassword(ResetPasswordDto resetPasswordDto, string token);
    }
}