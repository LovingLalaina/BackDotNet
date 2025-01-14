using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace back_dotnet.Models.DTOs.Chat;

public class MessageDto
{
    public MessageDto()
    {

    }
    
    public MessageDto(MessageDto otherMessageDto)
    {
        IdUserSender = otherMessageDto.IdUserSender;
        Content = otherMessageDto.Content;
        CreatedAt = otherMessageDto.CreatedAt;
    }

    [Required(ErrorMessage = "L'identifiant de l'utilisateur qui envoye le message n'est pas fourni")]
    [JsonPropertyName("id_user_sender")]
    public Guid IdUserSender { get; set; }

    [Required(ErrorMessage = "Aucun message n'a été envoyé")]
    [Length(1, 255, ErrorMessage = "Le message à envoyer est trop long (max : 255 caractères)")]
    public string Content { get; set; } = null!;

    [JsonPropertyName("send_at")]
    public DateTime? CreatedAt { get; set; }
}