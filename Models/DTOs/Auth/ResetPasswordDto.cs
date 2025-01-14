using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace back_dotnet.Models.DTOs.Auth
{
    public class ResetPasswordDto
    {
        [Required(ErrorMessage = "Le mot de passe est requis")]
        [MinLength(8, ErrorMessage = "Votre mot de passe doit contenir au moins 8 caractères")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "Votre mot de passe est très vulnérable. Il doit contenir au moins une lettre majuscule, une lettre minuscule, un chiffre, et un symbole.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "La confirmation du mot de passe est requise")]
        [Compare("Password", ErrorMessage = "Le mot de passe et la confirmation ne correspondent pas")]
        public string ConfirmPassword { get; set; }
    }
}