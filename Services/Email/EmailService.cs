using back_dotnet.Models.Domain;
using HandlebarsDotNet;
using DotNetEnv;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace back_dotnet.Services.Email;
public class EmailService : IEmailService
{
    public EmailService()
    {
        Env.Load("./.env");
    }
    public string TemplateResetPassword(User user, string token)
    {
        var routeFrontEnd = Environment.GetEnvironmentVariable("FRONT_END_BASE_ROUTE");
        var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "reset-password.hbs");
        var templateContent = System.IO.File.ReadAllText(templatePath);
        var template = Handlebars.Compile(templateContent);
        var data = new
        {
            username = user.Lastname,
            context = $"{routeFrontEnd}reset-password?token={token}"
        };
        var renderedContent = template(data);
        return renderedContent;
    }

    public string TemplateSendPassword(string Username, string password)
    {
        var routeFrontEnd = Environment.GetEnvironmentVariable("FRONT_END_BASE_ROUTE");
        var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "notification-password.hbs");
        var templateContent = System.IO.File.ReadAllText(templatePath);
        var template = Handlebars.Compile(templateContent);
        var data = new
        {
            username = Username,
            context = password,
            resetPwLink = $"{routeFrontEnd}forgot-password"
        };
        var renderedContent = template(data);
        return renderedContent;
    }

    public async Task SendEmailAsync(string recipientEmail, string subject, string body)
    {
        try
        {
            var message = new MimeMessage();
            var emailFrom = Environment.GetEnvironmentVariable("MAIL_USER");
            var emailHost = Environment.GetEnvironmentVariable("MAIL_HOST");
            var emailPort = Environment.GetEnvironmentVariable("MAIL_PORT");
            var emailPassword = Environment.GetEnvironmentVariable("MAIL_PASSWORD");
            message.From.Add(new MailboxAddress("HairunTechnology", emailFrom));
            message.To.Add(new MailboxAddress("", recipientEmail));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = body };
            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                if( emailPort == null )
                    throw new Exception("Erreur lors de l'envoi du mail'");
                await client.ConnectAsync( emailHost , int.Parse(emailPort), SecureSocketOptions.StartTls );
                await client.AuthenticateAsync(emailFrom, emailPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
        catch( Exception ex )
        {
            Console.WriteLine( "erreur ??");
            throw new Exception("Erreur lors de la generation de Token", ex);
        }
    }
}
