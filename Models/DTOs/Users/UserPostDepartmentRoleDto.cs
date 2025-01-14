using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using back_dotnet.Models.DTOs.Role;

namespace back_dotnet.Models.DTOs.Users
{
    public class UserPostDepartmentRoleDto
    {
         public Guid Id { get; set; }
        public string Name { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime? UpdatedAt { get; set; }
        public List<UserPostDepartmentRolePermissionDto> Permissions { get; set; }
    }
}