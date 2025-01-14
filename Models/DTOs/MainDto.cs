using System.Text.Json.Serialization;

namespace back_dotnet.Models.DTOs
{
    public class MainDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }
        [JsonPropertyName("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}
