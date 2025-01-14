using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace back_dotnet.Models.DTOs.Files
{
  public class GetFileDto
  {
    public Guid Id { get; set; }
    
    public string Name { get; set; } = null!;

    public string Path { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string? Size { get; set; }

    [JsonPropertyName("public_id")]
    public string PublicId { get; set; } = null!;

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTime? UpdatedAt { get; set; }
  }
}