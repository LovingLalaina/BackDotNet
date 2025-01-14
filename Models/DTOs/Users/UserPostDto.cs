using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using back_dotnet.Models.DTOs.Department;
using back_dotnet.Models.DTOs.Post;

namespace back_dotnet.Models.DTOs.Users
{
    public class UserPostDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }
        [JsonPropertyName("updated_at")]
        public DateTime? UpdatedAt { get; set; }
        [JsonPropertyName("department")]
        public UserPostDepartmentDto? IdDepartmentNavigation { get; set; }
    }
}