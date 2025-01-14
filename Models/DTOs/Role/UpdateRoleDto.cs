using back_dotnet.Attributes;
using System.ComponentModel.DataAnnotations;

namespace back_dotnet.Models.DTOs.Role
{
    public class UpdateRoleDto
    {
        [Length(4, 255, ErrorMessage = "Le nom doit avoir au moins 4 à 255 caractères et éviter les espaces successives")]
        public string Name { get; set; }

        [UuidCollectionAttribute(ErrorMessage = "Champ invalide (type : UUID)")]
        public ICollection<string> Permissions { get; set; }
    }
}
