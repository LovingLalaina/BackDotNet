using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using back_dotnet.Models.Domain;
using Microsoft.AspNetCore.Identity;

namespace back_dotnet.Repositories.Auth
{
    public interface IAuthRepository
    {
        Task<User> FindByUsernameAsync(string username);
        public Task<string> recoveryPassword( string email );
        public Task SendNotificationPassword(string username, string email, string password);
    }
}