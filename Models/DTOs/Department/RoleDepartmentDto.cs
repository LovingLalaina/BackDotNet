using System.Text.Json.Serialization;

namespace back_dotnet.Models.DTOs.Department
{
    public class RoleDepartmentDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}