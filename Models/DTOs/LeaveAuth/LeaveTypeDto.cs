using System.Text.Json.Serialization;

namespace back_dotnet.Models.DTOs.LeaveAuth;

public partial class LeaveTypeDto
{
    [JsonPropertyName("id_leave_type")]
    public Guid Id { get; set; }

    public string Designation { get; set; } = null!;
}