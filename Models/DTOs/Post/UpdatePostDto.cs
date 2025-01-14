using System.ComponentModel.DataAnnotations;
using back_dotnet.Attributes;
using back_dotnet.Utils;

namespace back_dotnet.Models.DTOs.Post
{
    public class UpdatePostDto
    {
        [TitleCase]
        [Required(ErrorMessage = "Le nom du poste est requis")]
        [Length(4, 255, ErrorMessage = "Vérifier la longueur de la valeur du champ [4-255].")]
        public string Name { get; set; } = null!;
        [IsUuid(ErrorMessage = "Veuillez inserer un departement valide")]
        public string Department { get; set; } = null!;
    }
}