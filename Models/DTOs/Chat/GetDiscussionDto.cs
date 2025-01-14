using System.Text.Json.Serialization;

namespace back_dotnet.Models.DTOs.Chat;

public class GetDiscussionDto
{
    [JsonPropertyName("id_discussion")]
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public string Avatar { get; set; } = "";

    [JsonPropertyName("participant_number")]
    public int ParticipantNumber { get; set; }

    public List<GetMessageDto>? Messages { get; set;}
}