using System.Text.Json.Serialization;
using back_dotnet.Models.DTOs.Files;

namespace back_dotnet.Models.DTOs.Chat;

public class GetMessageDto
{
    [JsonPropertyName("id_message")]
    public Guid Id { get; set; }

    [JsonPropertyName("id_sender")]
    public Guid IdUser { get; set; }

    public string Title { get; set; } = null!;
    
    [JsonPropertyName("image")]
    public Domain.File? Profil { get; set; }

    public string Content { get; set; } = null!;

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("is_read")]
    public bool IsRead { get; set; }

}