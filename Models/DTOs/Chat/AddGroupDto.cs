using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace back_dotnet.Models.DTOs.Chat;

public class AddGroupDto
{
    [Required(ErrorMessage = "L'identifiant de la discussion où ajouter l'utilisateur n'est pas fournie")]
    [JsonPropertyName("id_discussion")]
    public Guid IdDiscussion { get; set; }

    [Required(ErrorMessage = "L'identifiant de l'utilisateur à ajouter au groupe n'est pas fournie")]
    [JsonPropertyName("id_user")]
    public Guid IdUser { get; set; }
}