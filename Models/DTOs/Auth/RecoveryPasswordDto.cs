using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace back_dotnet.Models.DTOs.Auth;

public class RecoveryPasswordDto
{
    [Required(ErrorMessage = "Veuillez fournir le mail pour la récupération du mot de passe")]
    [RegularExpression(@"^.+@(hairun-technology)+\.(com)$", ErrorMessage = "Le mail doit se terminer par @hairun-technology.com")]
    public string Email { get; set; }
}