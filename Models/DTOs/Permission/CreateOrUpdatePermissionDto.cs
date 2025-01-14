using System.ComponentModel.DataAnnotations;

namespace back_dotnet.Models.DTOs.Permission
{
    public class CreateOrUpdatePermissionDto
    {

        [Length(4, 255, ErrorMessage = "Vérifier la longueur de la valeur du champ [4-255].")]
        [Required(ErrorMessage = "Vérifier que le champ existe bien.")]
        public string Name { get; set; } = null!;
    }
}
