
using System.Text.Json.Serialization;

namespace back_dotnet.Models.DTOs.LeaveAuth;

public partial class LeaveAuthorizationResponseDto
{
    [JsonPropertyName("id_leave_type")]
    public Guid IdLeaveType { get; set; }

    public string Designation { get; set; } = null!;

    public string Description { get; set; } = null!;

    [JsonPropertyName("start_validity")]
    public DateTime StartValidity { get; set; }

    [JsonPropertyName("end_validity")]
    public DateTime EndValidity { get; set; }

    public decimal Solde { get; set; }
}