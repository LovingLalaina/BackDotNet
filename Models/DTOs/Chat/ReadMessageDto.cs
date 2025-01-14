using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace back_dotnet.Models.DTOs.Chat;

public class ReadMessageDto
{
    [Required(ErrorMessage = "L'identifiant de l'utilisateur qui lit la discussion n'est pas fournie")]
    [JsonPropertyName("id_user")]
    public Guid IdUser { get; set; }

    [Required(ErrorMessage = "L'identifiant de la discussion à marquer lue n'est pas fournie")]
    [JsonPropertyName("id_discussion")]
    public Guid IdDiscussion { get; set; }
}