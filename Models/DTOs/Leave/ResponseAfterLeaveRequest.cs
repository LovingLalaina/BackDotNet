using System.Text.Json.Serialization;

namespace back_dotnet.Models.DTOs.Leave;

public class ResponseAfterLeaveRequest
{
    [JsonPropertyName("id_leave_")]
    public Guid IdLeave { get; set; }

    [JsonPropertyName("id_leave_type")]
    public Guid IdLeaveType { get; set; }

    [JsonPropertyName("id_user")]
    public Guid IdUser { get; set; }

    [JsonPropertyName("start_date")]
    public DateTime StartDate { get; set; }

    [JsonPropertyName("end_date")]
    public DateTime EndDate { get; set; }

    [JsonPropertyName("solde_after")]
    public decimal SoldeAfter { get; set; }
}