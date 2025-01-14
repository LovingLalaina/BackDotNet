using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace back_dotnet.Models.DTOs.Users
{
    public class UserPostDepartmentRolePermissionDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}