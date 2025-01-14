using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using back_dotnet.Models.Domain;
using back_dotnet.Models.DTOs.Auth;

namespace back_dotnet.Services.Email;

public interface IEmailService
{
    public string TemplateResetPassword(User user, string token);

    public string TemplateSendPassword(string Username, string password);

    public Task SendEmailAsync(string recipientEmail, string subject, string body);
}