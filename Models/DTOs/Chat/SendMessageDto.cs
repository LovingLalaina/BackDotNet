using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace back_dotnet.Models.DTOs.Chat;

public class SendMessageDto : MessageDto
{
    [JsonConstructor]
    public SendMessageDto()
    {
        // UTILISE POUR LA SERIALISATION JSON DANS LE PROCESSUS DE REQUETE HTTP
    }

    public SendMessageDto( StartDiscussionDto startDiscussionDto, Guid idDiscussion) : base( startDiscussionDto )
    {
        IdDiscussion = idDiscussion;
    }

    [Required(ErrorMessage = "L'identifiant de la discussion sur laquelle envoyer le message n'est pas fourni")]
    [JsonPropertyName("id_discussion")]
    public Guid IdDiscussion { get; set; }

    // CHAMPS DE MessageDto
    // [Required(ErrorMessage = "L'identifiant de l'utilisateur qui envoye le message n'est pas fourni")]
    // [JsonPropertyName("id_user_sender")]
    // public Guid IdUserSender { get; set; }

    // [Required(ErrorMessage = "Aucun message n'a été envoyé")]
    // [Length(1, 255, ErrorMessage = "Le message à envoyer est trop long (max : 255 caractères)")]
    // public string Content { get; set; } = null!;

    // [JsonPropertyName("send_at")]
    // public DateTime? CreatedAt { get; set; }
}