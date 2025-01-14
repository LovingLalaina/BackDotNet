using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace back_dotnet.Models.Domain;

public partial class User
{
    [Key]
    public Guid Uuid { get; set; }

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public DateTime BirthDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int Id { get; set; }

    public string Matricule { get; set; } = null!;

    public Guid IdPost { get; set; }

    public Guid? IdFile { get; set; }

    public string Email { get; set; } = null!;

    public string? Password { get; set; }

    [JsonPropertyName("file")]
    public virtual File? IdFileNavigation { get; set; }

    [JsonPropertyName("post")]
    public virtual Post IdPostNavigation { get; set; } = null!;

    public virtual ICollection<Leave> Leaves { get; set; } = new List<Leave>();

    public virtual ICollection<LeaveAuthorization> LeavesAuthorization { get; set; } = new List<LeaveAuthorization>();

    public virtual ICollection<UserDiscussion> UserDiscussions { get; set; } = new List<UserDiscussion>();
}
