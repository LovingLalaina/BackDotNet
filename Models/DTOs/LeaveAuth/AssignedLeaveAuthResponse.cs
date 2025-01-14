
using System.Text.Json.Serialization;

namespace back_dotnet.Models.DTOs.LeaveAuth;

public class AssignedLeaveAuthResponse
{
    [JsonPropertyName("id_user")]
    public Guid IdUser { get; set; }

    [JsonPropertyName("new_leave_auth")]
    public List<LeaveAuthorizationResponseDto> LeavesAuthorization  { get; set; } = null!;
}