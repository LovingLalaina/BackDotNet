using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using back_dotnet.Models.Domain;
using back_dotnet.Models.DTOs.Post;

namespace back_dotnet.Models.DTOs.Department
{
    public class DepartmentDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [JsonPropertyName("role")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public RoleDepartmentDto IdRoleNavigation { get; set; } = null!;
        public List<PostDto> Posts { get; set; } = null!;
    }
}