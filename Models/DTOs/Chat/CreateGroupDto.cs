using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace back_dotnet.Models.DTOs.Chat;

public class CreateGroupDto
{    
    [Required(ErrorMessage = "Aucun titre de groupe n'a été donné")]
    [Length(1, 255, ErrorMessage = "Le titre du groupe est trop long (max : 255 caractères)")]
    public string Title { get; set; } = null!;

    [Required(ErrorMessage = "Les identifiants des participants ne sont pas fournie")]
    public List<Guid> IdsParticipants { get; set; } = new List<Guid>();

    public string? Description { get; set; }
}