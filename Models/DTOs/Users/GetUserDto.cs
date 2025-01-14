using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using back_dotnet.Models.Domain;
using back_dotnet.Models.DTOs.Files;
using back_dotnet.Models.DTOs.Post;
using File = back_dotnet.Models.Domain.File;

namespace back_dotnet.Models.DTOs.Users
{
  public class GetUserDto
  {
    public Guid Uuid { get; set; }

    public int Id { get; set; }

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    [JsonPropertyName("birth_date")]
    public DateTime BirthDate { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    public string Matricule { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Password { get; set; }

    [JsonPropertyName("image")]
    public GetFileDto? IdFileNavigation { get; set; }

    [JsonPropertyName("post")]
    public  UserPostDto IdPostNavigation { get; set; } = null!;
  }
}