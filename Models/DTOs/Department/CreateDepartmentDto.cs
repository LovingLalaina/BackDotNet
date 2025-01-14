using System.ComponentModel.DataAnnotations;
using back_dotnet.Attributes;
using back_dotnet.Models.Domain;
using back_dotnet.Utils;

namespace back_dotnet.Models.DTOs.Department
{
    public class CreateDepartmentDto
    {
        [TitleCase]
        [Required(ErrorMessage = "Le nom du département est requis")]
        [Length(4, 255, ErrorMessage = "Le nom doit etre superieur ou égal à 4 et inferieur à 255")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "Le rôle doit être renseigner et valide", AllowEmptyStrings = false)]
        [IsUuid(ErrorMessage = "Le rôle doit être valide")]
        public string Role { get; set; } = null!;
    }
}