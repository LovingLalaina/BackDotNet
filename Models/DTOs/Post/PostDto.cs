using System.Text.Json.Serialization;

namespace back_dotnet.Models.DTOs.Post
{
    public class PostDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }
        [JsonPropertyName("updated_at")]
        public DateTime? UpdatedAt { get; set; }
        [JsonPropertyName("department")]
        public PostDepartmentDto IdDepartmentNavigation { get; set; } = null!;
    }
}