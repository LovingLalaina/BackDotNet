using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace back_dotnet.Models.DTOs.Auth
{
    public class LoginRequestDto
    {
        [Required(ErrorMessage ="Vérifier que le champ \"E-mail\" existe bien.")]
        [RegularExpression(@"^.+@(hairun-technology)+\.(com)$", ErrorMessage = "Le mail doit se terminer par @hairun-technology.com")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Le format de l'adresse n'est pas valide.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vérifier que le champ \"Mot de passe\" existe bien.")]
        [DataType(DataType.Password, ErrorMessage = "Le mot de passe doit être renseigné.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Vérifier que l'utilisateur a bien mentionné s'il veut qu'on se souvienne de lui.")]
        public string Duration { get; set; }
    }
}