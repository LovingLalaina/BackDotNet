using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using back_dotnet.Models.DTOs.Auth;
using back_dotnet.Services.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using back_dotnet.Exceptions;
using back_dotnet.Utils;
using System.Security.Claims;
using back_dotnet.Repositories.Users;
using AutoMapper;
using back_dotnet.Models.DTOs.Users;
using Newtonsoft.Json;
using back_dotnet.Services.Token;

namespace back_dotnet.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        private readonly ITokenService _tokenService;

        public AuthController(IAuthService authService, ITokenService tokenService)
        {
            _authService = authService;
            _tokenService = tokenService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            if (!TryValidateModel(loginRequestDto))
            {
                var ErrorValidationResponse = ValidationResponse.GetResponseValidation(ModelState);

                return StatusCode(StatusCodes.Status422UnprocessableEntity, new { status = StatusCodes.Status422UnprocessableEntity, error = ErrorValidationResponse });
            }
            try
            {
                var authResponse = await _authService.LoginAsync(loginRequestDto);

                return Ok(authResponse);
            }

            catch (HttpException error)
            {
                return StatusCode(error.Status, new { error.Status, error = error.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = StatusCodes.Status500InternalServerError, error = ex.Message });

            }
        }

        [HttpPost]
        [Route("recovery-password")]
        public async Task<IActionResult> RecoveryPassword([FromBody] RecoveryPasswordDto recoveryPasswordDto)
        {
            if (!TryValidateModel(recoveryPasswordDto))
            {
                var ErrorValidationResponse = ValidationResponse.GetResponseValidation(ModelState);

                return StatusCode(StatusCodes.Status422UnprocessableEntity, new { status = StatusCodes.Status422UnprocessableEntity, error = ErrorValidationResponse });
            }

            try
            {
                string tokenSimple = await _authService.recoveryPassword(recoveryPasswordDto);
                return Ok(new { isSending = true, token = tokenSimple });
            }
            catch (Exception ex)
            {
                var innerException = ex as HttpException;
                if (innerException is not null)
                    return StatusCode(innerException.Status, new { status = innerException.Status, error = innerException.Message });

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("resend-password")]
        public async Task<IActionResult> SendNotificationPassword([FromQuery] string username, [FromQuery] string email)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email))
                return StatusCode(StatusCodes.Status400BadRequest, "username et email requis");
            try
            {
                string tokenSimple = await _authService.recoveryPassword(new RecoveryPasswordDto{ Email = email});
                return Ok( new{ isSending = true, token = tokenSimple});
            }
            catch (Exception ex)
            {
                var innerException = ex as HttpException;
                if (innerException is not null)
                    return StatusCode(innerException.Status, new { status = innerException.Status, error = innerException.Message });

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("decode-token")]
        public async Task<IActionResult> DecodeToken([FromBody] TokenRequestDto request)
        {
            Claim? userClaim;
            try
            {
                userClaim = _tokenService.DecodeToken(request.Token, Tokens.TOKEN_KEY, false).Claims.FirstOrDefault(c => c.Type == "user");
                if(userClaim == null)
                    return BadRequest(new { authorized = false, message = "Le token ne contient pas les informations de l'utilisateur" } );
                
                GetUserDto? userFromToken = JsonConvert.DeserializeObject<GetUserDto>(userClaim.Value);
                return Ok( new { authorized = true, decodedToken = new { user = userFromToken } } );
            }
            catch (JsonSerializationException)
            {
                return BadRequest(new { authorized = false, message = "Les informations de l'utilisateur dans le token ne sont pas valides" });
            }
            catch (Exception)
            {
                return Unauthorized("Expiré");
            }
        }

        [HttpPost]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            try
            {
                var token = Request.Headers["token_password"].ToString();
                await _authService.ResetUserPassword(resetPasswordDto, token);
                return Accepted(new { message = "Mot de passe changé avec succès" });
            }
            catch( UnprocessableEntityException constraintsError)
            {
                return StatusCode(constraintsError.Status, new { status = constraintsError.Status, error = constraintsError.constraints } );
            }
            catch (Exception ex)
            {
                var innerException = ex as HttpException;
                if (innerException is not null)
                {
                    return StatusCode(innerException.Status, new { status = innerException.Status, error = innerException.Message });
                }
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("resend-mail-check-token")]
        public async Task<IActionResult> VerifyResendMail([FromHeader(Name = "token_resend_mail")] string token)
        {
            if (string.IsNullOrEmpty(token))
                return BadRequest("Token non fourni");

            if( await _authService.VerifyResendMail( token ) )
                return Accepted( new { message = "Autorisé à accéder à la page" } );
            return StatusCode( 410, new { status = 410, error = "Impossible d'accéder à cette page"});
        }

        [HttpPost("forgot-password-check-url")]
        public async Task<IActionResult> VerifyURLPassword([FromHeader(Name = "token_password")] string token)
        {
            if (string.IsNullOrEmpty(token))
                return BadRequest("Token non fourni");

            if( await _authService.VerifyURLPassword( token ) )
                return Accepted( new { message = "Autorisé à accéder à la page" } );
            return StatusCode( 410, new { status = 410, error = "Ce lien de réinitialisation du mot de passe est expiré,veuillez rééssayer de nouveau"});
        }
    }
}