using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace back_dotnet.Models.DTOs.Chat;

public class StartDiscussionDto : MessageDto
{
    [JsonConstructor]
    public StartDiscussionDto()
    {
        // UTILISE POUR LA SERIALISATION JSON DANS LE PROCESSUS DE REQUETE HTTP
    }

    public StartDiscussionDto(MessageDto otherMessageDto) : base(otherMessageDto)
    {
        //INUTILISE MAIS DOIT ETRE PLACER CAR UTILISE DANS LE PARENT MessageDto
    }

    [Required(ErrorMessage = "L'identifiant de l'utilisateur pour envoyer le message n'est pas fourni")]
    [JsonPropertyName("id_user_receiver")]
    public Guid IdUserReceiver { get; set; }

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