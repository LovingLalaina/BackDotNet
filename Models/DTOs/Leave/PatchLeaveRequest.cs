
using System.Text.Json.Serialization;
using back_dotnet.Models.Domain;

namespace back_dotnet.Models.DTOs.Leave;

public class PatchLeaveRequest
{
    [JsonPropertyName("id_leave")]
    public Guid Id { get; set; }

    [JsonPropertyName("new_status")]
    public LeaveStatus Status { get; set; }
}